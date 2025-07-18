using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public float ThrustAmount;
    public float RotationAmount => rb.angularVelocity.magnitude;

    private ShipActions controls;
    private Rigidbody rb;
    private PullBeam pb;
    private bool gamePaused = false;

    [SerializeField] public GameObject spaceship;

    [Header("Settings")]
    public float movementForce = 20f;
    public float rollForce = 10f;
    public float rotationSensitivity => 1f * UIManager.instance.sensFromSlider;

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
    private float boostTimer;
    private float boostDuration;
    private bool previouslyBoosting = false;

    [Header("Brake")]
    public float brakeForce = 50f;
    public bool isBraking;
    private bool previouslyBraking;

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
        pb = spaceship.GetComponentInChildren<PullBeam>();

        if (rb == null)
        {
            Debug.Log("Rigidbody for spaceship is null");
        }

        boostDuration = AudioManager.Instance.continuousSFX[(int)ContinuousSFX.Boost].length;
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
        // boost
        bool boostInput = controls.Flight.Boost.ReadValue<float>() > 0.1f;
        lookInput = controls.Flight.Look.ReadValue<Vector2>();

        if (boostInput && !previouslyBoosting && !isBoosting && boostCoroutine == null)
        {
            isBoosting = true;
            boostCoroutine = StartCoroutine(GetBoost());
        }

        if (controls.Flight.Pause.IsPressed() && !gamePaused)
        {
            UIManager.instance.PauseGame();
        }
        else if (controls.Flight.Pause.IsPressed() && gamePaused)
        {
            UIManager.instance.ResumeGame();
        }

            previouslyBoosting = boostInput;

        // brake
        isBraking = controls.Flight.Brake.ReadValue<float>() > 0.1f;
        HandleBraking();
        if(!previouslyBraking && isBraking) AudioManager.Instance.PlaySFX(OneShotSFX.Brake);
        previouslyBraking = isBraking;


        HandlePullBeam();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float thrustInput = controls.Flight.Thrust.ReadValue<float>();
        float strafeInput = controls.Flight.Strafe.ReadValue<float>();

        float boostFactor = isBoosting ? currentBoost : 1f;

        // apply forces
        rb.AddForce(spaceship.transform.forward * thrustInput * movementForce * boostFactor);
        rb.AddForce(spaceship.transform.right * strafeInput * movementForce);

        // calculate and store thrust amount
        ThrustAmount = (Mathf.Abs(thrustInput) + Mathf.Abs(strafeInput)) * movementForce * boostFactor;

        // use for audio
        AudioManager.Instance.UpdateThrusterSFX(ThrustAmount);
    }


    private void HandleRotation()
    {
        float pitchInput = lookInput.y * rotationSensitivity * PitchMultiplier;
        float yawInput = lookInput.x * rotationSensitivity * YawMultiplier;
        float rollInput = controls.Flight.Roll.ReadValue<float>() * rollForce * -RollMultiplier;

        Vector3 torqueVector = new Vector3(pitchInput, yawInput, rollInput);
        rb.AddRelativeTorque(torqueVector, ForceMode.Force);

        AudioManager.Instance.UpdateRotateSFX(RotationAmount);
    }

    private void HandlePullBeam()
    {
        pb.isPulling = controls.Flight.Magnetise.ReadValue<float>() > 0;
        AudioManager.Instance.UpdateMagnetizeSFX(pb.isPulling);

        bool isEjectPressed = controls.Flight.Release.triggered;

        if (isEjectPressed && pb.HeldBody != null)
        {
            pb.EjectBody(pb.HeldBody);
        }
    }

    private void HandleBraking()
    {
        if (isBraking && rb.velocity.magnitude > 0.1f)
        {
            Vector3 brakingForce = -rb.velocity.normalized * brakeForce;
            rb.AddForce(brakingForce, ForceMode.Force);
        }
    }

    private IEnumerator GetBoost()
    {
        boostTimer = 0f;

        while (true)
        {
            if (ThrustAmount > 0)
            {
                // update timer
                boostTimer += Time.deltaTime;

                // stop if its too long
                bool boostInput = controls.Flight.Boost.ReadValue<float>() > 0.1f;
                if (boostTimer >= boostDuration || !boostInput)
                {
                    Debug.Log("end boost");
                    break;
                }

                // increase power
                currentBoost += boostRate * Time.deltaTime;
                currentBoost = Mathf.Clamp(currentBoost, 1f, maxBoost);
            }
            else
            {
                // fade out
                currentBoost = Mathf.MoveTowards(currentBoost, 1f, boostRate * Time.deltaTime);
            }

            float normalized = Mathf.InverseLerp(1f, maxBoost, currentBoost);
            CameraManager.Instance.SetBoostAmount(normalized);
            AudioManager.Instance.UpdateBoosterSFX(normalized);

            yield return null;
        }

        // end boost
        isBoosting = false;
        boostCoroutine = null;
        currentBoost = 1f;

        CameraManager.Instance.SetBoostAmount(0f);
        AudioManager.Instance.UpdateBoosterSFX(0f);
    }
}