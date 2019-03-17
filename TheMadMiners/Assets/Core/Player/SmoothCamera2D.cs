using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{

    public float dampTime = 0.15f;
    [HideInInspector]
    public Transform target;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private const float defaultOrthographicSize = 6f;
    private bool ShakeOnce = false;
    
    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (target && PlayerHealth.singleton.isAlive)
        {
            cam.orthographicSize = defaultOrthographicSize;
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else if(!target && PlayerHealth.singleton.isAlive)
        {
            if (GameManager.singleton.player != null)
            {
                target = GameManager.singleton.player.transform;
            }
        }
        else if (!PlayerHealth.singleton.isAlive && !ShakeOnce)
        {
            ShakeOnce = true;
            GetComponent<CameraShake>().StartShaking();
        }
    }
}