using UnityEngine;
using System.Collections.Generic;

public class Rocket : MonoBehaviour
{
    public Vector2 velocity; // Current velocity of the player
    public float maxSpeed = 200f; // Maximum speed of the player
    public float dumpFactor = 0.5f; // Dumping factor for velocity
    public float engineForce = 10f; // Force applied by the engine
    public float sideEngineForce = 5f; // Force applied by the side engines

    private List<Vector2> gravityForces = new List<Vector2>(); // Store forces for visualization
    private float distanceFromCenterToBottom = 0.5f; // Distance from the center of mass to the bottom of the rocket
    private float distanceFromCenterToSide = 0.5f; // Distance from the center of mass to the side of the rocket


    void FixedUpdate()
    {
        gravityForces.Clear(); // Reset forces for this frame

        // Get combined gravity from the GravityManager
        Vector2 combinedGravity = Vector2.zero;
        foreach (var obj in GravityManager.Instance.GetGravityObjects())
        {
            Vector2 gravity = obj.CalculateGravityAtPoint(transform.position);
            gravityForces.Add(gravity); // Store individual gravity forces
            combinedGravity += gravity;
        }

        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(combinedGravity);

        RunEngine();
        rigidbody.linearVelocity = Vector2.ClampMagnitude(rigidbody.linearVelocity, maxSpeed);
    }

    void RunEngine()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        var engineForceVector = Vector3.zero;
        float torque = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            engineForceVector += transform.up * engineForce;
            torque += engineForce * -distanceFromCenterToBottom;
        }
        if (Input.GetKey(KeyCode.A))
        {
            engineForceVector += -transform.right * sideEngineForce;
            torque += sideEngineForce * distanceFromCenterToSide;
        }
        if (Input.GetKey(KeyCode.D))
        {
            engineForceVector += transform.right * sideEngineForce;
            torque += -sideEngineForce * distanceFromCenterToSide;
        }
        if (Input.GetKey(KeyCode.S))
        {
            engineForceVector += -transform.up * engineForce;
            torque += engineForce * distanceFromCenterToBottom;
        }
        rigidbody.AddForce((Vector2)engineForceVector);
        rigidbody.AddTorque(torque);
    }

    float CalculateTorque(Vector3 forceDirection)
{
    // Calculate the distance vector from the center of mass to the force application point (relative to the center of the rocket)
    Vector2 distance = transform.up * (transform.localScale.y / 2f); // Assuming the engine is at the top
    float torque = Vector2.Perpendicular(distance).magnitude * forceDirection.magnitude;

    return torque;
}

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Visualize gravity vectors
        Gizmos.color = Color.green; // Use green for gravity vectors
        foreach (var gravity in gravityForces)
        {
            DrawArrow(transform.position, gravity, Color.green);
        }
        // Draw velocity
        DrawArrow(transform.position, velocity, Color.blue);
    }

    private void DrawArrow(Vector2 start, Vector2 direction, Color color)
    {
        Gizmos.color = color;

        // Draw the arrow stem
        Vector2 end = start + direction;
        Gizmos.DrawLine(start, end);

        // Draw the arrowhead
        const float arrowHeadAngle = 30f;
        const float arrowHeadLength = 0.2f;

        Vector2 arrowDirection = direction.normalized;
        float arrowHeadSize = Mathf.Min(direction.magnitude, arrowHeadLength);

        Vector2 right = Quaternion.Euler(0, 0, arrowHeadAngle) * -arrowDirection * arrowHeadSize;
        Vector2 left = Quaternion.Euler(0, 0, -arrowHeadAngle) * -arrowDirection * arrowHeadSize;

        Gizmos.DrawLine(end, end + right);
        Gizmos.DrawLine(end, end + left);
    }
}
