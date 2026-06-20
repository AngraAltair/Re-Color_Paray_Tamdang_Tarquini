using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Script for the AudioPanel. Uses PlayerPrefs to load in audio settings on start up.
/// </summary>
public class AudioPanel : MonoBehaviour
{
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField] private AudioMixer bgmAudioMixer;
    [SerializeField] private AudioMixer sfxAudioMixer;

    [Header("Audio Sliders")]
    [SerializeField] private Slider masterAudioSlider;
    [SerializeField] private Slider bgmAudioSlider;
    [SerializeField] private Slider sfxAudioSlider;

    private float masterAudioVolume;
    private float bgmAudioVolume;
    private float sfxAudioVolume;

    // CONSTANT PLAYERPREF KEY NAMES
    private const string MASTER_AUDIO_MIXER_KEY = "MasterAudioVolume";
    private const string BGM_AUDIO_MIXER_KEY = "BGMAudioVolume";
    private const string SFX_AUDIO_MIXER_KEY = "SFXAudioVolume";

    /// <summary>
    /// Load in PlayerPrefs audio settings on startup and set slider values accordingly, if any exist.
    /// </summary>
    void Start()
    {
        if (PlayerPrefs.HasKey(MASTER_AUDIO_MIXER_KEY))
        {
            masterAudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat(MASTER_AUDIO_MIXER_KEY));
            masterAudioSlider.value = PlayerPrefs.GetFloat(MASTER_AUDIO_MIXER_KEY);
        }

        if (PlayerPrefs.HasKey(BGM_AUDIO_MIXER_KEY))
        {
            bgmAudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat(BGM_AUDIO_MIXER_KEY));
            bgmAudioSlider.value = PlayerPrefs.GetFloat(BGM_AUDIO_MIXER_KEY);
        }

        if (PlayerPrefs.HasKey(SFX_AUDIO_MIXER_KEY))
        {
            sfxAudioMixer.SetFloat("Volume", PlayerPrefs.GetFloat(SFX_AUDIO_MIXER_KEY));
            sfxAudioSlider.value = PlayerPrefs.GetFloat(SFX_AUDIO_MIXER_KEY);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Event for when the Master Volume slider's value is changed.
    /// </summary>
    public void OnMasterVolumeChange()
    {
        if (PlayerPrefs.HasKey(MASTER_AUDIO_MIXER_KEY))
        {
            masterAudioMixer.SetFloat("Volume", masterAudioSlider.value);
        }
        else
        {
            PlayerPrefs.SetFloat(MASTER_AUDIO_MIXER_KEY, masterAudioSlider.value);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Event for when the BGM Volume slider's value is changed.
    /// </summary>
    public void OnBGMVolumeChange()
    {
        if (PlayerPrefs.HasKey(BGM_AUDIO_MIXER_KEY))
        {
            bgmAudioMixer.SetFloat("Volume", bgmAudioSlider.value);
        }
        else
        {
            PlayerPrefs.SetFloat(BGM_AUDIO_MIXER_KEY, bgmAudioSlider.value);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Event for when the SFX Volume slider's value is changed.
    /// </summary>
    public void OnSFXVolumeChange()
    {
        if (PlayerPrefs.HasKey(SFX_AUDIO_MIXER_KEY))
        {
            sfxAudioMixer.SetFloat("Volume", sfxAudioSlider.value);
        }
        else
        {
            PlayerPrefs.SetFloat(SFX_AUDIO_MIXER_KEY, sfxAudioSlider.value);
        }
        PlayerPrefs.Save();
    }
}
