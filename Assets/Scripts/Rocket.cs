using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 velocity; // Current velocity of the player
    public float maxFallSpeed = -20f; // Limit on how fast the player can fall

    private void FixedUpdate()
    {
        // Get combined gravity from the GravityManager
        Vector2 gravity = GravityManager.Instance.GetCombinedGravity(transform.position);

        // Apply gravity to velocity
        velocity += gravity * Time.fixedDeltaTime;

        // Clamp the fall speed
        velocity.y = Mathf.Max(velocity.y, maxFallSpeed);

        // Update the player's position
        Vector2 newPosition = (Vector2)transform.position + velocity * Time.fixedDeltaTime;

        // Check for collisions with gravity-affecting objects
        foreach (var obj in GravityManager.Instance.GetGravityObjects())
        {
            if (obj.IsInside(newPosition))
            {
                // Correct the player's position to stay on the surface
                newPosition = obj.GetClosestPointOnSurface(newPosition);

                // Adjust velocity to prevent further inward movement
                Vector2 normal = (newPosition - (Vector2)obj.transform.position).normalized;
                velocity = Vector2.Reflect(velocity, normal);
            }
        }

        // Apply the new position
        transform.position = newPosition;
    }
}
