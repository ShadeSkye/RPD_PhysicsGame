using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEditor;

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

        if(virtualCamera  == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }


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

    public void OneShotShake(float shakeAmount = 2f, float duration = 2f)
    {
        StartCoroutine(OneShotShakeRoutine(shakeAmount, duration));
    }

    private IEnumerator OneShotShakeRoutine(float shakeAmount , float duration)
    {
        float originalAmplitude = noise.m_AmplitudeGain;
        float originalFrequency = noise.m_FrequencyGain;

        noise.m_AmplitudeGain = shakeAmount;
        noise.m_FrequencyGain = Mathf.Lerp(minFreq, maxFreq, shakeAmount) * 0.5f;

        yield return new WaitForSeconds(duration);

        noise.m_AmplitudeGain = originalAmplitude;
        noise.m_FrequencyGain = originalFrequency;
    }

}
