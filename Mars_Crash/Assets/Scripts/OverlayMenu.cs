using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OverlayMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

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
        SaveVolume();
    }
    public void MainMenu()
    {
        SceneSwitch.Instance.ScenenChanger("MainMenu");
        MusicManager.Instance.PlayMusic("MainMenu", 0.5f);
        SaveVolume();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }
    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}