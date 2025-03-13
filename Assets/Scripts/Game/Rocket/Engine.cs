using UnityEngine;

public class Engine : MonoBehaviour
{
    private Rigidbody2D parentRb;
    public KeyCode launchKey = KeyCode.Space;
    private bool isEngineActive = false;
    public float engineForce = 10f;

    void Start()
    {
        // Get the Rigidbody2D component from the parent object
        parentRb = GetComponentInParent<Rigidbody2D>();
        if (parentRb == null)
        {
            Debug.LogError("Parent Rigidbody2D not found!");
        }
    }

    void Update()
    {
        if (Input.GetKey(launchKey))
        {
            ActivateEngine();
        }
        else
        {
            DeactivateEngine();
        }
    }

    void ActivateEngine()
    {
        if (parentRb != null)
        {
            // Convert local position to world position
            Vector2 worldPosition = transform.position;
            parentRb.AddForceAtPosition(transform.up * engineForce, worldPosition);
            isEngineActive = true;
        }
    }

    void DeactivateEngine()
    {
        isEngineActive = false;
    }

    void OnDrawGizmos()
    {
        if (isEngineActive)
        {
            Gizmos.color = Color.red;
            Vector2 start = transform.position;
            Vector2 end = start + (Vector2)transform.up * engineForce * 0.1f; // Scale the arrow for better visualization
            Gizmos.DrawLine(start, end);

            // Draw arrowhead
            Vector2 right = Quaternion.Euler(0, 0, 20) * (end - start).normalized * 0.1f;
            Vector2 left = Quaternion.Euler(0, 0, -20) * (end - start).normalized * 0.1f;
            Gizmos.DrawLine(end, end + right);
            Gizmos.DrawLine(end, end + left);
        }
    }
}