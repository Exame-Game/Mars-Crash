using UnityEngine;

[System.Serializable]
public struct ParticleEffect
{
    public string effectName;
    public ParticleSystem particlePrefab;
}

public class ParticleLibrary : MonoBehaviour
{
    [SerializeField] private ParticleEffect[] particleEffects;

    public ParticleSystem GetParticleFromName(string effectName)
    {
        foreach (ParticleEffect effect in particleEffects)
            if (effect.effectName == effectName)
                return effect.particlePrefab;
        
        Debug.LogWarning("Particle effect not found: " + effectName);
        return null;
    }
}
