using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Shake")]
    [SerializeField] private float minShake;
    [SerializeField] private float maxShake;
    [SerializeField] private float minFreq;
    [SerializeField] private float maxFreq;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        else
        {
            Debug.LogWarning("virtual camera null");
        }
    }
    public void SetBoostAmount(float boostAmount)
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = Mathf.Lerp(minShake, maxShake, boostAmount); 
            noise.m_FrequencyGain = Mathf.Lerp(minFreq, maxFreq, boostAmount);
        }
    }

}
