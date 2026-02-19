using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _musicSlider;
    [SerializeField] private GameObject _sfxSlider;
    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _ufo;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _exitSettingsButton;
    
    void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Play()
    {
        SceneSwitch.Instance.ScenenChanger("Alienlevel");
        MusicManager.Instance.PlayMusic("Level 1", 0.5f);
    }

    public void Settings()
    {
        _startButton.SetActive(false);
        _ufo.SetActive(false);
        _settingsButton.SetActive(false);

        _exitSettingsButton.SetActive(true);
        _musicSlider.SetActive(true);
        _sfxSlider.SetActive(true);
    }
    public void ExitSettings()
    {
        _musicSlider.SetActive(false);
        _sfxSlider.SetActive(false);
        _exitSettingsButton.SetActive(false);
        
        _startButton.SetActive(true);
        _ufo.SetActive(true);
        _settingsButton.SetActive(true);
    }
}
