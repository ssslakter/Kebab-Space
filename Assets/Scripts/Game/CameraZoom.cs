using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;  // Speed of zoom
    public float minSize = 5f;  // Minimum camera size (zoom in limit)
    public float maxSize = 20f;  // Maximum camera size (zoom out limit)

    void Update()
    {
        var mainCamera = Camera.main;
        // Get the mouse scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // Adjust the camera's orthographic size based on scroll input
        mainCamera.orthographicSize -= scrollInput * zoomSpeed;

        // Clamp the size to avoid going beyond limits
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minSize, maxSize);
    }
}
