using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] private ParticleLibrary particleLibrary;

    private ParticleSystem _particlePrefabInstance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnParticleEffect(Vector3 position, Quaternion rotation, string effectName)
    {
        if (_particlePrefabInstance != null)
            Destroy(_particlePrefabInstance.gameObject);
        
        _particlePrefabInstance = Instantiate(particleLibrary.GetParticleFromName(effectName), position, rotation);
    }

    public void SpawnParticleEffect(GameObject target, string effectName)
    {
        SpawnParticleEffect(target.transform.position, target.transform.rotation, effectName);
    }

    public void SpawnParticleEffect(Transform transform, string effectName)
    {
        SpawnParticleEffect(transform.position, transform.rotation, effectName);
    }
}
