using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject tutorialPopupPanel;
    [SerializeField] private GameObject secondTutorialPopupPanel;

    [Header("Difficulty Groups")]
    [SerializeField] private RectTransform easyGroup;
    [SerializeField] private RectTransform mediumGroup;
    [SerializeField] private RectTransform hardGroup;

    [Header("Position Settings")]
    [SerializeField] private float upY = 0f;       // Y position when active/visible
    [SerializeField] private float downY = -300f;  // Y position when hidden below
    [SerializeField] private float slideDuration = 0.4f;

    private string pendingLevelScene;
    private const string PrefKey = "ShowTutorialPopup";
    private const string SecondPrefKey = "ShowSecondTutorialPopup";

    // Track which group is currently active
    private RectTransform activeGroup;

    private void Awake()
    {
        tutorialPopupPanel.SetActive(false);
        secondTutorialPopupPanel.SetActive(false);

        // Start with all groups at downY, hidden
        SetGroupY(easyGroup, downY);
        SetGroupY(mediumGroup, downY);
        SetGroupY(hardGroup, downY);

        // Disable all level buttons initially
        SetGroupInteractable(easyGroup, false);
        SetGroupInteractable(mediumGroup, false);
        SetGroupInteractable(hardGroup, false);

        // Activate easy group by default
        SwitchToGroup(easyGroup);
    }

    // --- Difficulty Button Callbacks ---

    public void OnEasySelected()
    {
        SwitchToGroup(easyGroup);
    }

    public void OnMediumSelected()
    {
        SwitchToGroup(mediumGroup);
    }

    public void OnHardSelected()
    {
        SwitchToGroup(hardGroup);
    }

    // --- Core Switch Logic ---

    private void SwitchToGroup(RectTransform selectedGroup)
    {
        // Slide down and disable all groups
        foreach (var group in new[] { easyGroup, mediumGroup, hardGroup })
        {
            if (group != selectedGroup)
            {
                StartCoroutine(SlideGroup(group, downY));
                SetGroupInteractable(group, false);
            }
        }

        // Slide up and enable the selected one
        StartCoroutine(SlideGroup(selectedGroup, upY));
        SetGroupInteractable(selectedGroup, true);

        activeGroup = selectedGroup;
    }

    // --- Animation ---

    private IEnumerator SlideGroup(RectTransform group, float targetY)
    {
        float elapsed = 0f;
        float startY = group.anchoredPosition.y;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration); // smooth ease in/out
            float newY = Mathf.Lerp(startY, targetY, t);
            group.anchoredPosition = new Vector2(group.anchoredPosition.x, newY);
            yield return null;
        }

        // Snap to exact target
        group.anchoredPosition = new Vector2(group.anchoredPosition.x, targetY);
    }

    // --- Helpers ---

    private void SetGroupY(RectTransform group, float y)
    {
        group.anchoredPosition = new Vector2(group.anchoredPosition.x, y);
    }

    private void SetGroupInteractable(RectTransform group, bool interactable)
    {
        foreach (var btn in group.GetComponentsInChildren<Button>())
            btn.interactable = interactable;
    }

    // --- Level Loading (your existing logic, untouched) ---

    public void OnFirstStageButton(string levelScene)
    {
        pendingLevelScene = levelScene;

        if (PlayerPrefs.GetInt(PrefKey, 1) == 1)
            tutorialPopupPanel.SetActive(true);
        else
            LoadScene(levelScene);
    }

    public void OnSecondStageButton(string levelScene)
    {
        pendingLevelScene = levelScene;

        if (PlayerPrefs.GetInt(SecondPrefKey, 1) == 1)
            secondTutorialPopupPanel.SetActive(true);
        else
            LoadScene(levelScene);
    }

    public void OnStageButton(string sceneName)
    {
        LoadScene(sceneName);
    }

    public void OnBackToMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void OnPopupYes()
    {
        PlayerPrefs.SetInt(PrefKey, 0);
        PlayerPrefs.Save();
        tutorialPopupPanel.SetActive(false);
        LoadScene("Tutorial");
    }

    public void OnPopupNo()
    {
        PlayerPrefs.SetInt(PrefKey, 0);
        PlayerPrefs.Save();
        tutorialPopupPanel.SetActive(false);
        LoadScene(pendingLevelScene);
    }

    public void ReenableTutorialPopup()
    {
        PlayerPrefs.SetInt(PrefKey, 1);
        PlayerPrefs.Save();
    }

    public void ShowSecondTutorialPopup()
    {
        if (PlayerPrefs.GetInt(SecondPrefKey, 1) == 1)
            secondTutorialPopupPanel.SetActive(true);
        else
            LoadScene("Tutorial2");
    }

    public void OnSecondPopupYes()
    {
        PlayerPrefs.SetInt(SecondPrefKey, 0);
        PlayerPrefs.Save();
        secondTutorialPopupPanel.SetActive(false);
        LoadScene("Tutorial2");
    }

    public void OnSecondPopupNo()
    {
        PlayerPrefs.SetInt(SecondPrefKey, 0);
        PlayerPrefs.Save();
        secondTutorialPopupPanel.SetActive(false);
        LoadScene(pendingLevelScene);
    }

    private void LoadScene(string sceneName)
    {
        if (LevelTransitioner.Instance != null)
            LevelTransitioner.Instance.TransitionToLevel(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}