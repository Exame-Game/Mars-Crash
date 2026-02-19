using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Play()
    {
        SceneSwitch.Instance.ScenenChanger("Alienlevel");
        MusicManager.Instance.PlayMusic("Level 1", 0.5f);
    }
}
