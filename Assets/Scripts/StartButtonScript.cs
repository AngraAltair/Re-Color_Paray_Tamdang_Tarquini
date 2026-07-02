using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    [SerializeField] private bool requireOptionsPanel = true;
    public CanvasGroup OptionsMenu;
    public GameObject optionsPanel;
    public CanvasGroup Menu;
    public GameObject menuPanel;

    public void Start()
    {
        if (requireOptionsPanel && OptionsMenu == null)
            OptionsMenu = optionsPanel.GetComponent<CanvasGroup>();

        SetCanvasGroupVisibility(OptionsMenu, false);
    }
    public void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void LoadModeSelection()
    {
        SceneManager.LoadScene("GameType");
    }

    public void LoadMultiplayerMode()
    {
        SceneManager.LoadScene("CreateServer");
        // Debug.Log("to load");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        SetCanvasGroupVisibility(OptionsMenu, true);
        SetCanvasGroupVisibility(Menu, false);
    }

    public void CloseOptions()
    {
        SetCanvasGroupVisibility(OptionsMenu, false);
        SetCanvasGroupVisibility(Menu, true);
    }

    private void SetCanvasGroupVisibility(CanvasGroup group, bool visible)
    {
        if (group == null) return;
        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }
}
