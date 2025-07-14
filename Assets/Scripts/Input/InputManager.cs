using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private ShipActions controls;
    private Rigidbody rb;

    [SerializeField] public GameObject spaceship;

    [Header("Settings")]
    public float movementForce = 20f;
    public float rollForce = 10f;
    public float rotationSensitivity = 5f;

    [Header("Controls")]
    public bool invertPitch; 
    public bool invertRoll; 
    public bool invertYaw;
    public int PitchMultiplier => invertPitch ? -1 : 1;
    public int YawMultiplier => invertYaw ? -1 : 1;
    public int RollMultiplier => invertRoll ? -1 : 1;

    private Vector2 lookInput;

    [Header("Boost")]
    public float maxBoost;
    public float boostRate;
    private float currentBoost;
    private bool isBoosting = false;
    private Coroutine boostCoroutine;

    // Beam
    public bool IsBeamHeld => controls.Flight.Magnetise.ReadValue<float>() > 0;
    public bool IsEjectPressed => controls.Flight.Release.triggered;

    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // optional: enforce singleton
            return;
        }

        Instance = this;

        // get references
        controls = new ShipActions();
        rb = spaceship.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("Rigidbody for spaceship is null");
        }
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        isBoosting = controls.Flight.Boost.ReadValue<float>() > 0.1f;

        if (isBoosting && boostCoroutine == null)
        {
            boostCoroutine = StartCoroutine(GetBoost());
        }
        else if (!isBoosting && boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
            boostCoroutine = null;
            currentBoost = 1f;
            CameraManager.Instance.SetBoostAmount(0f); // reset camera
        }

        lookInput = controls.Flight.Look.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float thrustInput = controls.Flight.Thrust.ReadValue<float>();
        rb.AddForce(spaceship.transform.forward * thrustInput * movementForce * (isBoosting ? currentBoost : 1f));

        float strafeInput = controls.Flight.Strafe.ReadValue<float>();
        rb.AddForce(spaceship.transform.right * strafeInput * movementForce);
    }


    private void HandleRotation()
    {
        float pitchInput = lookInput.y * rotationSensitivity * PitchMultiplier;
        float yawInput = lookInput.x * rotationSensitivity * YawMultiplier;
        float rollInput = controls.Flight.Roll.ReadValue<float>() * rollForce * -RollMultiplier;

        Vector3 torqueVector = new Vector3(pitchInput, yawInput, rollInput);
        rb.AddRelativeTorque(torqueVector, ForceMode.Force);
    }

    private IEnumerator GetBoost()
    {
        while (true)
        {
            currentBoost += boostRate * Time.deltaTime;
            currentBoost = Mathf.Clamp(currentBoost, 1f, maxBoost);

            float normalizedBoost = (currentBoost - 1f) / (maxBoost - 1f);
            normalizedBoost = Mathf.Clamp01(normalizedBoost);
            CameraManager.Instance.SetBoostAmount(normalizedBoost); // update camera

            yield return null;
        }
    }
}
