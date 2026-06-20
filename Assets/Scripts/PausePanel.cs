using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script specifically for controls found within the pause panel. Attached to the pause panel's parent object.
/// Requires an object LevelTransitioner if you want the transitions to play.
/// </summary>
public class PausePanel : MonoBehaviour
{
    public void ReturnToLevelSelect()
    {
        if (LevelTransitioner.Instance != null)
        {
            GameManager.Instance.TogglePausePanel();
            LevelTransitioner.Instance.TransitionToLevel("LevelSelection");
        } else
        {
            GameManager.Instance.TogglePausePanel();
            SceneManager.LoadScene("LevelSelection");
        }
    }
}
