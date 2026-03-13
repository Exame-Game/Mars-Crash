using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    
    private AudioSource _audioSource;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
    
    private void OnFootstep()
    {
        _audioSource.Play();
    }
}
