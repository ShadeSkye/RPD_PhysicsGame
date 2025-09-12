using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Damageable))]
public class PlayerManager : MonoBehaviour
{
    private Damageable dmg;
    private PullBeam pullBeam;

    private GravityBody lookingAtBody;
    private GravityBody holdingBody;

    private Camera mainCamera;

    [SerializeField] private float minImpactDrop = 5f;

    public float damagePercent
    {
        get => dmg.damagePercent;
        set => dmg.damagePercent = value;
    }
    private void Awake()
    {
        dmg = GetComponent<Damageable>();
        pullBeam = GetComponentInChildren<PullBeam>();

        mainCamera = Camera.main;

    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int mask = ~(1 << 2);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide))
        {
            //Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.TryGetComponent<GravityBody>(out GravityBody body))
            {
                float distance = Vector3.Distance(transform.position, body.transform.position);
                LookAtDisplay.Instance.UpdateLookAtObject(body.bodyName, distance);
            }

        }
        else
        {
            LookAtDisplay.Instance.ClearDisplay();
        }

        Debug.Log(dmg.damagePercent);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // get hit amount
        float impactAmount = collision.relativeVelocity.magnitude;

        // if above amount then eject
        if (impactAmount >= minImpactDrop)
        {
            if (pullBeam?.HeldBody != null)
            {
                pullBeam.UnlockBody(pullBeam.HeldBody);
            }

        }

        dmg.ApplyImpact(impactAmount);

        AudioManager.Instance.PlayOneShot("Crash");

    }

}
