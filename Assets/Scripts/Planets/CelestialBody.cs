using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CelestialBody : MonoBehaviour
{

    [Header("Simulation")]
    [SerializeField, Min(0)]
    public float radius = 5f;
    public float density = 1f; // Magnitude of gravity
    private float gravityRadius => radius * 50f; // Effective range of gravity
    private float gravityStrength => density * radius * radius;


    [SerializeField]
    private Shader circleShader;

    [Header("Rendering")]
    [SerializeField, Min(3)]
    private int segments = 100;
    public Color centerColor = Color.white; // Color of the center of the circle
    public Color edgeColor = Color.white;   // Color of the edge of the circle
    public float GradientPower = 1f;        // Power of the gradient effect

    private Material circleMaterial; // Material created from the shader



    void Start()
    {
        // Initialize shader and circle
        GenerateCircle();
        UpdateMaterial(circleShader);
    }

    void OnValidate()
    {
        if (TryGetComponent<CircleCollider2D>(out var collider))
        {
            collider.radius = radius;
        }
        if (circleShader != null)
        {
            UpdateMaterial(circleShader);
        }
        if (circleMaterial == null) return;
        circleMaterial.SetColor("_CenterColor", centerColor);
        circleMaterial.SetColor("_EdgeColor", edgeColor);
        circleMaterial.SetFloat("_GradPow", GradientPower);
    }

    private void OnEnable()
    {
        GravityManager.Instance.Register(this);
    }

    private void OnDisable()
    {
        GravityManager.Instance.Unregister(this);
    }



    void UpdateMaterial(Shader shader)
    {
        // Create or update material from the shader
        if (circleMaterial == null || circleMaterial.shader != shader)
        {
            circleMaterial = new Material(shader);
            if (TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                meshRenderer.material = circleMaterial;
            }
            else
            {
                Debug.LogError("MeshRenderer is missing!");
            }
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

    // Check if a point is inside the gravity radius
    public bool IsInside(Vector2 point)
    {
        float distance = Vector2.Distance(transform.position, point);
        return distance < radius;
    }

    // Get the closest point on the surface of the circle
    public Vector2 GetClosestPointOnSurface(Vector2 point)
    {
        Vector2 direction = (point - (Vector2)transform.position).normalized;
        return (Vector2)transform.position + direction * gravityRadius;
    }

}
