using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private VolumeSettings _volumeSettings;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;

    void Start()
    {
        _volumeSettings.LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Play()
    {
        SceneSwitch.Instance.SceneChanger("FirstLevel");
        MusicManager.Instance.PlayMusic("FirstLevel", 0.5f);
    }

    public void OpenSettings()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _volumeSettings.LoadVolume();
    }

    public void SaveAudioSettings()
    {
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _volumeSettings.SaveVolume();
    }
}