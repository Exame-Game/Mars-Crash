using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    //todo less references 
    [SerializeField] private GameObject _musicSlider;
    [SerializeField] private GameObject _sfxSlider;
    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _ufo;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _exitSettingsButton;
    
    void Start()
    {
        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Play()
    {
        SceneSwitch.Instance.ScenenChanger("Alienlevel");
        MusicManager.Instance.PlayMusic("Level 1", 0.5f);
    }

    public void OpenSettings()
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
