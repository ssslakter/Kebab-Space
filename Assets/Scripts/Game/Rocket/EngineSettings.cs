using UnityEngine;

public class EngineSettingsManager : MonoBehaviour
{
    public float engineForce = 10f;           // Shared engine force value
    public ParticleSystem engineParticleSystem; // Shared particle system, could be assigned via Inspector

    // Singleton pattern to ensure only one instance exists
    public static EngineSettingsManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
