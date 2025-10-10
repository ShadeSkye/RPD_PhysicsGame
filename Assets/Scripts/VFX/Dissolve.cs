using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    Material[] defaultMats;
    Material[] instanceMats;

    public float Duration = 2f;
    private Material dissolveMat;

    void Start()
    {
        dissolveMat = Resources.Load<Material>("Materials/BaseDissolve");

        defaultMats = new Material[renderers.Length];
        instanceMats = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            defaultMats[i] = renderers[i].sharedMaterial;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) DoEffect();
        if (Input.GetKey(KeyCode.Alpha2)) ResetMaterials();
    }

    public void DoEffect()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (instanceMats[i] == null)
            {
                Material instance = new Material(dissolveMat);
                instance.SetFloat("Duration", Duration);

                Texture albedoTex = defaultMats[i].GetTexture("_BaseMap") ?? Texture2D.blackTexture;
                instance.SetTexture("_BaseTex", albedoTex);
                instanceMats[i] = instance;
            }

            renderers[i].material = instanceMats[i];
            instanceMats[i].SetFloat("_StartTime", Time.time);
        }
    }
    public void ResetMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = defaultMats[i];
        }
    }
}
