using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Cargo
{
    protected void Explode()
    {
        Debug.LogError("EXPLODE");
    }
}