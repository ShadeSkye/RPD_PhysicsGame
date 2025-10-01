using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnstableBomb : Bomb
{
    [Header("Proximity Trigger")]
    private Coroutine triggerRoutine;
    [SerializeField] private float triggerTime = 1f;
    [SerializeField] private float triggerRadius = 5f;
    private int objectsInTrigger = 0;

    public bool OnlyAffectsPlayer = false;

    protected override void Awake()
    {
        base.Awake();

        TriggerUnstableBomb trigger = GetComponentInChildren<TriggerUnstableBomb>();
        if (trigger != null)
        {
            trigger.Init(this);
            SphereCollider col = trigger.GetComponent<SphereCollider>();
            col.isTrigger = true;
            
            trigger.gameObject.transform.localScale = Vector3.one * triggerRadius * 2f;
        }
    }

    public void HandleTriggerEnter(Collider other)
    {
        objectsInTrigger++;

        if(objectsInTrigger == 1 && triggerRoutine == null)
        {
            //Debug.Log("Object in explosion radius, started countdown");
            triggerRoutine = StartCoroutine(TriggerCountdown());
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        objectsInTrigger--;

        if(objectsInTrigger == 0 && triggerRoutine != null)
        {
            //Debug.Log("Object left explosion radius, stopped countdown");
            StopCoroutine(triggerRoutine);
            triggerRoutine = null;
        }
    }

    private IEnumerator TriggerCountdown()
    {
        float timer = triggerTime;
        
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            //Debug.Log(timer + this.gameObject.name);
            yield return null;       
            
        }

        if (objectsInTrigger > 0)
        {
            Explode();
        }

        triggerRoutine = null;
        
    }
}
