using System.Numerics;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlePrefab;


    private ParticleSystem _particlePrefabInstance;

    public void SpawnParticleEffect(Transform particleTransform)
    {
        if (_particlePrefabInstance != null)
            Destroy(_particlePrefabInstance.gameObject);
        
        _particlePrefabInstance = Instantiate(_particlePrefab, particleTransform.position, particleTransform.rotation);
    }

    public void SpawnParticleEffect(GameObject target)
    {
        if (_particlePrefabInstance != null)
            Destroy(_particlePrefabInstance.gameObject);
        
        _particlePrefabInstance = Instantiate(_particlePrefab, target.transform.position, target.transform.rotation);
    }

    
}
    
        

   
