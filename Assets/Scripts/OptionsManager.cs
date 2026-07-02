using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("Indicators")]
    [SerializeField] private TMP_Text masterIndicator;
    [SerializeField] private TMP_Text bgmIndicator;
    [SerializeField] private TMP_Text sfxIndicator;

    [Header("Buttons")]
    [SerializeField] private Button[] masterButtons;
    [SerializeField] private Button[] bgmButtons;
    [SerializeField] private Button[] sfxButtons;
    [SerializeField] private Button reenableTutorialButtons;

    private Color activeColor = Color.red;
    private Color[] masterOriginalColors;
    private Color[] bgmOriginalColors;
    private Color[] sfxOriginalColors;

    private readonly float[] buttonValues = 
    {
        0.1f, 0.2f, 0.3f, 0.4f, 0.5f,
        0.65f, 0.8f, 0.95f, 0.975f, 1.0f
    };

    [Header("Fullscreen")]
    [SerializeField] private Toggle fullscreenToggle;
    private int lastWidth;
    private int lastHeight;

    private void Awake()
    {
        masterOriginalColors = CacheOriginalColors(masterButtons);
        bgmOriginalColors = CacheOriginalColors(bgmButtons);
        sfxOriginalColors = CacheOriginalColors(sfxButtons);
    }

    private void Update()
    {
        if (!Screen.fullScreen && (Screen.width != lastWidth || Screen.height != lastHeight))
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            PlayerPrefs.SetInt("WindowWidth", lastWidth);
            PlayerPrefs.SetInt("WindowHeight", lastHeight);
            PlayerPrefs.Save();
        }

        bool levelSelectorExists = LevelSelector.Instance != null;
        if (reenableTutorialButtons != null)
            reenableTutorialButtons.interactable = levelSelectorExists;
    }

    private void Start()
    {
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        ApplyFullscreen(fullscreen);

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.RemoveListener(ToggleFullscreen);
            fullscreenToggle.isOn = fullscreen;
            fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);
        }

        lastWidth = Screen.width;
        lastHeight = Screen.height;

        int masterIndex = PlayerPrefs.GetInt("MasterIndex", 8);
        int bgmIndex    = PlayerPrefs.GetInt("BGMIndex", 8);
        int sfxIndex    = PlayerPrefs.GetInt("SFXIndex", 8);

        SetMasterVolume(masterIndex);
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

    public void SetMasterVolume(int buttonIndex)
    {
        float value = buttonValues[buttonIndex - 1];
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("MasterVol", volume);
        masterIndicator.text = buttonIndex.ToString();
        UpdateButtonColors(masterButtons, masterOriginalColors, buttonIndex);
        PlayerPrefs.SetInt("MasterIndex", buttonIndex);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(int buttonIndex)
    {
        float value = buttonValues[buttonIndex - 1];
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("BGMVol", volume);
        bgmIndicator.text = buttonIndex.ToString();
        UpdateButtonColors(bgmButtons, bgmOriginalColors, buttonIndex);
        PlayerPrefs.SetInt("BGMIndex", buttonIndex);
        PlayerPrefs.Save();

    }

    public void SetSFXVolume(int buttonIndex)
    {
        float value = buttonValues[buttonIndex - 1];
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("SFXVol", volume);
        sfxIndicator.text = buttonIndex.ToString();
        UpdateButtonColors(sfxButtons, sfxOriginalColors, buttonIndex);
        PlayerPrefs.SetInt("SFXIndex", buttonIndex);
        PlayerPrefs.Save();
    }

    private void UpdateButtonColors(Button[] buttons, Color[] originals, int buttonIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var colors = buttons[i].colors;
            // Light up all buttons up to the pressed one
            colors.normalColor = (i < buttonIndex) ? activeColor : originals[i];
            buttons[i].colors = colors;
        }
    }

    public void ToggleFullscreen(bool isOn)
    {
        ApplyFullscreen(isOn);
        PlayerPrefs.SetInt("Fullscreen", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyFullscreen(bool fullscreen)
    {
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            int width  = PlayerPrefs.GetInt("WindowWidth", 1280);
            int height = PlayerPrefs.GetInt("WindowHeight", 720);
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(width, height, false);
        }

        lastWidth  = Screen.width;
        lastHeight = Screen.height;
    }


    public void ResetProgressFromOptions()
    {
        if (LevelSelector.Instance != null)
        {
            LevelSelector.Instance.ResetAllStageProgress();
        }
        else
        {
            Debug.LogWarning("LevelSelector instance not found!");
        }
    }

    public void OnReenableAllTutorials()
    {
        PlayerPrefs.SetInt("ShowTutorialPopup", 1);
        PlayerPrefs.SetInt("ShowSecondTutorialPopup", 1);
        PlayerPrefs.Save();
        Debug.Log("Both tutorial popups reenabled.");
    }



    public void UnlockStagesFromOptions()
    {
        if (LevelSelector.Instance != null)
        {
            LevelSelector.Instance.UnlockAllStagesForTesting();
        }
        else
        {
            Debug.LogWarning("LevelSelector instance not found!");
        }
    }
}
