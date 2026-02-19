using UnityEngine;

public class OverlayMenu : MonoBehaviour
{
    [SerializeField] private VolumeSettings _volumeSettings;
    [SerializeField] private GameObject _overlayMenu;
    [SerializeField] private GameObject _overlayMenuButton;


    public void Open()
    {
        _overlayMenu.SetActive(true);
        _overlayMenuButton.SetActive(false);
        _volumeSettings.LoadVolume();
    }

    public void Close()
    {
        _overlayMenu.SetActive(false);
        _overlayMenuButton.SetActive(true);
        _volumeSettings.SaveVolume();
    }
    public void MainMenu()
    {
        SceneSwitch.Instance.SceneChanger("MainMenu");
        MusicManager.Instance.PlayMusic("MainMenu", 0.5f);
        _volumeSettings.SaveVolume();
    }

    public void TestSFX()
    {
        SoundManager.Instance.PlaySound2D("ButtonClick");
    }
}