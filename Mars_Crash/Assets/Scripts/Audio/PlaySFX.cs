using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] private string sfxName;

    public void PlaySound()
    {
        SoundManager.Instance.PlaySound2D(sfxName);
    }
}