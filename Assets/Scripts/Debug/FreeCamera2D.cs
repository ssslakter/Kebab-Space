using UnityEngine;

public class FreeCamera2D : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float zoomSpeed = 10f;
    public float minZoom = 2f;
    public float maxZoom = 20f;

    void Update()
    {
        // Camera movement
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveX, moveY, 0);

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
    }
}
