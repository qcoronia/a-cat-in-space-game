using System.Collections;
using UnityEngine;

public class CameraAVFX : MonoBehaviour
{
    [Header("Shake")]
    public float shakeIntensity;
    public float shakeDecreaseRate;
    public float shakeDamping;
    private float currentShakeIntensity = 0f;

    [Header("Events")]
    public EventHandler onHuggedRockHitIce;

    private Camera cameraObj;

    void Start()
    {
        cameraObj = GetComponentInChildren<Camera>();

        if (onHuggedRockHitIce)
        {
            onHuggedRockHitIce.OnInvoke.AddListener(Shake);
        }
    }

    public void Shake()
    {
        currentShakeIntensity = shakeIntensity;
        StartCoroutine(ShakeLoop());
    }

    IEnumerator ShakeLoop()
    {
        while (currentShakeIntensity > 0.01f)
        {
            var targetPos = Vector2.Lerp(
                Vector2.zero,
                Random.insideUnitCircle * currentShakeIntensity,
                Random.value
            );

            cameraObj.transform.localPosition = Vector2.Lerp(
                cameraObj.transform.localPosition,
                targetPos,
                shakeDamping * Time.smoothDeltaTime
            );

            currentShakeIntensity = Mathf.Lerp(
                currentShakeIntensity,
                0f,
                shakeDecreaseRate * Time.deltaTime
            );
            yield return new WaitForEndOfFrame();
        }

        cameraObj.transform.localPosition = Vector2.zero;
    }
}
