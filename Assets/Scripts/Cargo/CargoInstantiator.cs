using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoInstantiator : MonoBehaviour
{
    public static CargoInstantiator Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }


}
