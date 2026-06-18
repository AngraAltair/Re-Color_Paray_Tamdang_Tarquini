using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    /// <summary>
    /// Call this function from your UI Button's OnClick() event.
    /// Pass the exact string name of the level scene you want to load.
    /// </summary>
    public void SelectLevel(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("LevelSelector: Scene name is empty!");
            return;
        }
        if (LevelTransitioner.Instance != null)
        {
            Debug.Log($"Triggering rainbow transition to: {sceneName}");
            LevelTransitioner.Instance.TransitionToLevel(sceneName);
        }
        else
        {
            Debug.LogWarning("LevelTransitioner instance not found. Performing instant scene load.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}