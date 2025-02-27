using UnityEngine;
using System.Collections.Generic;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;

    private List<GravityAffectingObject> gravityObjects = new List<GravityAffectingObject>();

    private void Awake()
    {
        // Ensure only one instance of GravityManager exists
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Add a gravity object to the list
    public void Register(GravityAffectingObject obj)
    {
        if (!gravityObjects.Contains(obj))
            gravityObjects.Add(obj);
    }

    // Remove a gravity object from the list
    public void Unregister(GravityAffectingObject obj)
    {
        if (gravityObjects.Contains(obj))
            gravityObjects.Remove(obj);
    }

    // Calculate total gravity affecting a point in space (e.g., the player)
    public Vector2 GetCombinedGravity(Vector2 position)
    {
        Vector2 totalGravity = Vector2.zero;

        foreach (var obj in gravityObjects)
        {
            totalGravity += obj.CalculateGravityAtPoint(position);
        }

        return totalGravity;
    }

    // **Expose the list of gravity objects**
    public List<GravityAffectingObject> GetGravityObjects()
    {
        return gravityObjects;
    }
}
