using UnityEngine;

public class CameraOrbiting : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float zoomMin = 5;
    public float zoomMax = 10;
    public float dragSpeed = 6f;

    void Update()
    {
        dragSpeed = GetComponent<Camera>().orthographicSize * 4;
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }
  
        GetComponent<Camera>().orthographicSize *= (Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed + 1);

        if(GetComponent<Camera>().orthographicSize < zoomMin)
        {
            GetComponent<Camera>().orthographicSize = zoomMin;
        }
        if (GetComponent<Camera>().orthographicSize > zoomMax)
        {
            GetComponent<Camera>().orthographicSize = zoomMax;
        }
    }
}