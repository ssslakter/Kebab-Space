using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target; // Automatically set to this object
    public Vector3 offset; // Offset from the target
    public float smoothSpeed = 1f; // Smooth follow speed

    void Start()
    {
        target = transform; // Set target to the current object
        offset = new Vector3(0, 0, -10); // Set offset to a default value
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, smoothSpeed);
            Camera.main.transform.position = smoothedPosition;
        }
    }
}
