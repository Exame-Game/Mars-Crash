using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private VolumeSettings _volumeSettings;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _ufo;

    private void Start()
    {
        _volumeSettings.LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Play()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        SceneSwitchManager.Instance.SwitchScene("FirstLevel");
        MusicManager.Instance.PlayMusic("FirstLevel", 0.5f);
    }

    public void OpenSettings()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        _mainMenu.SetActive(false);
        _ufo.SetActive(false);
        _settingsMenu.SetActive(true);
        _volumeSettings.LoadVolume();
    }

    public void SaveAudioSettings()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        _settingsMenu.SetActive(false);
        _mainMenu.SetActive(true);
        _ufo.SetActive(true);
        _volumeSettings.SaveVolume();
    }
}
