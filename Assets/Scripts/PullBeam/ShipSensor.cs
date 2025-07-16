using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ShipSensor : MonoBehaviour
{
    PullBeam pullBeam;
    [SerializeField] private float minImpact = 5f;

    private GravityBody lookingAtBody;
    private GravityBody holdingBody;


    private void Awake()
    {
        pullBeam = GetComponentInChildren<PullBeam>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        // get hit amount
        float impactAmount = collision.relativeVelocity.magnitude;
        //Debug.LogWarning($"Hit {collision.gameObject} for {impactAmount} force");

        // if above amount then eject
        if (impactAmount >= minImpact && pullBeam?.HeldBody != null)
        {
            pullBeam.UnlockBody(pullBeam.HeldBody);
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit: " + hit.collider.name);

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
    }
}
