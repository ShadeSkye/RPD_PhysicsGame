using System.Collections;
using UnityEngine;

public class ShipDamageFlash : MonoBehaviour
{
    public static ShipDamageFlash Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [SerializeField] private Transform target;

    [SerializeField] private float baseMin = 1.3f;
    [SerializeField] private float max = 1f;
    private float min => Mathf.Lerp(baseMin, max, PlayerManager.Instance.damagePercent);

    [SerializeField] private float duration = 0.1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TriggerFlash();
        }
    }

    public void TriggerFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        target.transform.localScale = Vector3.one * max;

        float timer = 0f;
        float returnDuration = duration;
        Vector3 startScale = target.transform.localScale;
        Vector3 endScale = Vector3.one * min;

        while (timer < returnDuration)
        {
            timer += Time.deltaTime;
            float t = timer / returnDuration;
            t = t * t;
            target.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        target.transform.localScale = endScale;
    }
}
