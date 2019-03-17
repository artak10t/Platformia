using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    public float shakeDuration = 1f;
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    private bool canShake = false;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    public void StartShaking()
    {
        canShake = true;
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0 && canShake)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }
}