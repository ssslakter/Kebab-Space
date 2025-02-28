using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CelestialBody))]
public class CircleMeshGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        CelestialBody generator = (CelestialBody)target;

        DrawDefaultInspector();

        if (GUI.changed)
        {
            // This whole editor exists because of stupid unity behavior which does not allow to update the mesh in OnValidate
            generator.GenerateCircle();
        }
    }
}
