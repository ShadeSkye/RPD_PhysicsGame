using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUnstableBomb : MonoBehaviour
{
    private UnstableBomb bomb;

    public void Init(UnstableBomb parentBomb)
    {
         bomb = parentBomb;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!DetectCollision(other)) return;

        Debug.Log(other + " Entered");
        bomb.HandleTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!DetectCollision(other)) return;

        Debug.Log(other + " Exited");
        bomb.HandleTriggerExit(other);
    }

    private bool DetectCollision(Collider other)
    {
        if (other.CompareTag("CollisionIgnore")) return false;
        else if (!other.CompareTag("Player") && bomb.OnlyAffectsPlayer) return false;
        else return true;
    }
}
