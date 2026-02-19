using UnityEngine;

public class OverlayMenu : MonoBehaviour
{
    [SerializeField] private GameObject _overlayMenu;
    [SerializeField] private GameObject _overlayMenuButton;


    public void Open()
    {
        _overlayMenu.SetActive(true);
        _overlayMenuButton.SetActive(false);
    }

    public void Close()
    {
        _overlayMenu.SetActive(false);
        _overlayMenuButton.SetActive(true);
    }
    public void MainMenu()
    {
        SceneSwitch.Instance.ScenenChanger("MainMenu");
        MusicManager.Instance.PlayMusic("MainMenu", 0.5f);
    }
}