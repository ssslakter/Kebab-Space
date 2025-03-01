using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Rocket : MonoBehaviour
{
    public Vector2 velocity; // Current velocity of the player
    public float G = 6.67430e-11f; // Gravitational constant
    public float maxSpeed = 200f; // Maximum speed of the player
    public float dumpFactor = 0.5f; // Dumping factor for velocity
    public float engineForce = 10f; // Force applied by the engine
    public float sideEngineForce = 5f; // Force applied by the side engines

    private List<Vector2> gravityForces = new List<Vector2>(); // Store forces for visualization


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

        // RunEngine();
        rigidbody.linearVelocity = Vector2.ClampMagnitude(rigidbody.linearVelocity, maxSpeed);
    }

    void Start()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        var planet = GravityManager.Instance.GetGravityObjects().FirstOrDefault();
        if (planet != null)
        {
            // Calculate the initial velocity to escape the planet
            var planetMass = planet.density * Mathf.PI * planet.radius * planet.radius;
            var rocketToPlanet = transform.position - planet.transform.position;
            var escapeSpeed = Mathf.Sqrt(G* planetMass / rocketToPlanet.magnitude);
            var tangent = -new Vector2(-rocketToPlanet.y, rocketToPlanet.x).normalized;
            rigidbody.linearVelocity = tangent * escapeSpeed;
        }

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
