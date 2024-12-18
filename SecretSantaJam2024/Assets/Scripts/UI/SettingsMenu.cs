using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public TMP_Dropdown qualityDropdown;
    public AudioClip buttonClickClip;
    public GameObject settingsMenu;

    private void Start()
    {
        // Load saved values from PlayerPrefs
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // Set initial volume based on saved values
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);

        // Add listeners to the sliders
        musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChange(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSFXSliderChange(); });

        // Initialize and set the quality dropdown
        InitializeQualityDropdown();
    }

    public void OnMusicSliderChange()
    {
        float volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void OnSFXSliderChange()
    {
        float volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        AudioManager.Instance.SetSFXVolume(volume);
    }

    private void InitializeQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        List<string> options = new List<string>();

        // Add quality levels to the dropdown options
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            options.Add(QualitySettings.names[i]);
        }

        qualityDropdown.AddOptions(options);
        qualityDropdown.value = PlayerPrefs.GetInt("QualitySetting", QualitySettings.GetQualityLevel());
        qualityDropdown.RefreshShownValue();

        qualityDropdown.onValueChanged.AddListener(delegate { OnQualityChange(); });
    }

    public void OnQualityChange()
    {
        int qualityIndex = qualityDropdown.value;
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualitySetting", qualityIndex);
        PlayerPrefs.Save();
    }

    public void BackButton()
    {
        settingsMenu.SetActive(false);
    }
}
