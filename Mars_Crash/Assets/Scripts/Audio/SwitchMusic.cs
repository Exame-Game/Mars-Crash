using UnityEngine;

public class SwitchMusic : MonoBehaviour
{
    [SerializeField] private string trackName;
    [SerializeField] private float fadeDuration = 1f;
    
    public void SwitchTrack()
    {
        MusicManager.Instance.PlayMusic(trackName, fadeDuration);
    }
}
