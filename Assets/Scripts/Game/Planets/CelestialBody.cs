using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody2D))]
public class CelestialBody : MonoBehaviour
{

    [Min(0)]
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed";


    [Header("Rendering")]
    [Min(3)]
    public int segments = 100;

    public Vector3 velocity { get; private set; }
    public float mass { get; private set; }
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = initialVelocity;
        RecalculateMass();
        GenerateCircle();
    }


    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;

                Vector3 acceleration = forceDir * Universe.gravitationalConstant * otherBody.mass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        rb.MovePosition(rb.position + (Vector2)velocity * timeStep);

    }

    public void RecalculateMass()
    {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
        if (rb) rb.mass = mass;
    }


    void OnValidate()
    {
        RecalculateMass();
        if (TryGetComponent<CircleCollider2D>(out var collider))
        {
            collider.radius = radius;
        }
    }

    Mesh GenerateMesh()
    {
        Vector3[] vertices = new Vector3[segments + 1]; // +1 for the center point
        Vector2[] uv = new Vector2[segments + 1];        // UV coordinates for gradient effect
        int[] triangles = new int[segments * 3];  // Each segment is represented by 2 triangles (3 indices per triangle)

        // Set the center vertex
        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0f, 0f); // Center UV

        // Create the vertices around the circle
        float angleStep = 2 * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            uv[i + 1] = new Vector2(1, 1);
        }

        // Create the triangles
        for (int i = 0; i < segments; i++)
        {
            int currentVertex = i + 1;
            int nextVertex = (i + 1) % segments + 1;  // Wrap around to 0 for the last vertex
            triangles[i * 3] = 0;  // Center of the circle
            triangles[i * 3 + 1] = nextVertex;
            triangles[i * 3 + 2] = currentVertex;
        }

        return new Mesh
        {
            vertices = vertices,
            uv = uv,
            triangles = triangles
        };
    }

    public void GenerateCircle()
    {
        // Create mesh
        var mesh = GenerateMesh();

        // Recalculate normals and bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set the mesh to the MeshFilter
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }


    public Rigidbody2D Rigidbody
    {
        get
        {
            if (!rb)
            {
                rb = GetComponent<Rigidbody2D>();
            }
            return rb;
        }
    }

    public Vector3 Position
    {
        get
        {
            return rb.position;
        }
    }

}
