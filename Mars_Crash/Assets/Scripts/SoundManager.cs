using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private SoundLibrary sfxLibrary;
    [SerializeField] private AudioSource sfx2DSource;

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

    public void PlaySound3D(Vector3 position, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Attempted to play a null audio clip.");
            return;
           
        }

        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlaySound3D(Vector3 position, string soundName)
    {
        PlaySound3D( position, sfxLibrary.GetClipFromName(soundName));
    }

    public void PlaySound2D(string soundName)
    {
       sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
}
