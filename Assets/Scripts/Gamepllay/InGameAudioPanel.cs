using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class InGameAudioPanel : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("Indicators")]
    [SerializeField] private TMP_Text bgmIndicator;
    [SerializeField] private TMP_Text sfxIndicator;

    [Header("Buttons")]
    [SerializeField] private Button[] bgmButtons;
    [SerializeField] private Button[] sfxButtons;

    private Color activeColor = Color.red;
    private Color[] bgmOriginalColors;
    private Color[] sfxOriginalColors;

    private void Awake()
    {
        bgmOriginalColors = CacheOriginalColors(bgmButtons);
        sfxOriginalColors = CacheOriginalColors(sfxButtons);
    }

    private void Start()
    {
        // Load saved values (default 8 ≈ 0.95f)
        int bgmIndex = PlayerPrefs.GetInt("BGMIndex", 8);
        int sfxIndex = PlayerPrefs.GetInt("SFXIndex", 8);

        SetBGMVolume(bgmIndex);
        SetSFXVolume(sfxIndex);
    }

    private Color[] CacheOriginalColors(Button[] buttons)
    {
        Color[] originals = new Color[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
            originals[i] = buttons[i].colors.normalColor;
        return originals;
    }

    public void SetBGMVolume(int buttonIndex)
    {
        float value = buttonIndex / 10f;
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        audioMixer.SetFloat("BGMVol", volume);

        bgmIndicator.text = buttonIndex.ToString();

        PlayerPrefs.SetInt("BGMIndex", buttonIndex);
        PlayerPrefs.Save();

        UpdateButtonColors(bgmButtons, bgmOriginalColors, buttonIndex);
    }

    public void SetSFXVolume(int buttonIndex)
    {
        float value = buttonIndex / 10f;
        float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        audioMixer.SetFloat("SFXVol", volume);

        sfxIndicator.text = buttonIndex.ToString();

        PlayerPrefs.SetInt("SFXIndex", buttonIndex);
        PlayerPrefs.Save();

        UpdateButtonColors(sfxButtons, sfxOriginalColors, buttonIndex);
    }

    private void UpdateButtonColors(Button[] buttons, Color[] originals, int buttonIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i < buttonIndex) ? activeColor : originals[i];
            buttons[i].colors = colors;
        }
    }
}
