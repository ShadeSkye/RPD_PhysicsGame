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
        if (other.CompareTag("CollisionIgnore")) return;
        bomb.HandleTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollisionIgnore")) return;
        Debug.Log(other + " Exited");
        bomb.HandleTriggerExit(other);
    }
}
