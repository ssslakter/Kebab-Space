using UnityEngine;

public class Engine : MonoBehaviour
{
    private Rigidbody2D parentRb;
    public KeyCode launchKey = KeyCode.Space;
    private bool isEngineActive = false;

    void Start()
    {
        // Get the Rigidbody2D component from the parent object
        parentRb = GetComponentInParent<Rigidbody2D>();
        if (parentRb == null)
        {
            Debug.LogError("Parent Rigidbody2D not found!");
        }

        if (EngineSettingsManager.Instance.engineParticleSystem == null)
        {
            Debug.LogError("Engine particles not assigned in EngineSettingsManager!");
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
        if (!EngineSettingsManager.Instance.engineParticleSystem.isPlaying)
        {
            EngineSettingsManager.Instance.engineParticleSystem.Play();
        }
        if (parentRb != null)
        {
            // Convert local position to world position
            Vector2 worldPosition = transform.position;
            parentRb.AddForceAtPosition(transform.up * EngineSettingsManager.Instance.engineForce, worldPosition);
            isEngineActive = true;
        }
    }

    void DeactivateEngine()
    {
        if (EngineSettingsManager.Instance.engineParticleSystem.isPlaying)
        {
            EngineSettingsManager.Instance.engineParticleSystem.Stop();
        }
        isEngineActive = false;
    }

    void OnDrawGizmos()
    {
        if (isEngineActive)
        {
            Gizmos.color = Color.red;
            Vector2 start = transform.position;
            Vector2 end = start + (Vector2)transform.up * EngineSettingsManager.Instance.engineForce * 0.1f; // Scale the arrow for better visualization
            Gizmos.DrawLine(start, end);

            // Draw arrowhead
            Vector2 right = Quaternion.Euler(0, 0, 20) * (end - start).normalized * 0.1f;
            Vector2 left = Quaternion.Euler(0, 0, -20) * (end - start).normalized * 0.1f;
            Gizmos.DrawLine(end, end + right);
            Gizmos.DrawLine(end, end + left);
        }
    }
}