using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource musicSource;
    private AudioSource[] sfxSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        musicSource = GameObject.Find("MusicSource")?.GetComponent<AudioSource>();
        if (musicSource == null)
        {
            Debug.LogError("MusicSource GameObject or AudioSource component is missing.");
        }

        sfxSources = FindObjectsOfType<AudioSource>();
        if (sfxSources == null || sfxSources.Length == 0)
        {
            Debug.LogWarning("No SFX AudioSources found in the scene.");
        }

        // Set initial volumes based on saved preferences
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SetMusicVolume(musicVolume);

        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        SetSFXVolume(sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSources != null)
        {
            foreach (var sfxSource in sfxSources)
            {
                if (sfxSource != musicSource)
                {
                    sfxSource.volume = volume;
                }
            }
        }
    }
}
