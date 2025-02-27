using UnityEngine;

public class GravityAffectingObject : MonoBehaviour
{
    public float gravityStrength = 9.8f; // Magnitude of gravity
    public float gravityRadius = 5f; // Area of effect

    private void OnEnable()
    {
        GravityManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        GravityManager.Instance.Unregister(this);
    }

    // Calculate gravity at a given position
    public Vector2 CalculateGravityAtPoint(Vector2 point)
    {
        Vector2 direction = (Vector2)transform.position - point;
        float distance = direction.magnitude;

        // If outside the effective range, no gravity
        if (distance > gravityRadius) return Vector2.zero;

        // Gravity weakens with distance (e.g., inverse-square law)
        float gravityEffect = gravityStrength / (distance * distance);
        return direction.normalized * gravityEffect;
    }

    public Vector2 GetClosestPointOnSurface(Vector2 point)
    {
        Vector2 direction = (point - (Vector2)transform.position).normalized;
        return (Vector2)transform.position + direction * gravityRadius;
    }

    public bool IsInside(Vector2 point)
    {
        float distance = Vector2.Distance(transform.position, point);
        return distance < gravityRadius;
    }

}
