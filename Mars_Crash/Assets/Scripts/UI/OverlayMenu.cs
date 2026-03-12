using UnityEngine;

public class OverlayMenu : MonoBehaviour
{
    [SerializeField] private VolumeSettings _volumeSettings;
    [SerializeField] private GameObject _overlayMenu;
    [SerializeField] private GameObject _overlayMenuButton;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _endScreenPanel;

    public void OpenSettings()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        _overlayMenu.SetActive(true);
        _overlayMenuButton.SetActive(false);
        _volumeSettings.LoadVolume();
    }

    public void CloseSettings()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        _overlayMenu.SetActive(false);
        _overlayMenuButton.SetActive(true);
        _volumeSettings.SaveVolume();
    }

    public void MainMenu()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        SceneSwitchManager.Instance.SwitchScene("MainMenu");
        MusicManager.Instance.PlayMusic("MainMenu", 0.5f);
        _volumeSettings.SaveVolume();
    }

    public void OpenTuturial()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        _overlayMenu.SetActive(false);
        _tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        _tutorialPanel.SetActive(false);
        _overlayMenu.SetActive(true);
    }

    public void Endscreen()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
        _endScreenPanel.SetActive(true);
    }
}
