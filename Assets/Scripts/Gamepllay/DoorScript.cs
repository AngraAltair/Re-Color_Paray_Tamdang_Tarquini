using System.Collections;
using UnityEngine;
// If you are loading a new Unity scene, you'll need this:
using UnityEngine.SceneManagement; 

public class DoorScript : MonoBehaviour
{
    [Header("Children Components")]
    [Tooltip("The Animator component attached to your 'Lock' child object")]
    [SerializeField] private Animator lockAnimator;
    
    [Tooltip("The Animator component attached to your 'DoorToOpen' child object")]
    [SerializeField] private Animator doorAnimator;

    [Header("Animation State Names")]
    [SerializeField] private string lockStateName = "Lock";
    [SerializeField] private string openStateName = "Open";

    [Header("Level Transition Settings")]
    [Tooltip("The exact name of the Unity Scene you want to load next")]
    [SerializeField] private string nextSceneName = "Level2"; 
    
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";

    private bool hasOpened = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Guard clauses: Stop if already opened or if it's not the player
        if (hasOpened) return;
        if (!other.CompareTag(playerTag)) return;

        // Check if the player actually has the key
        if (PlayerManager.Instance != null && PlayerManager.Instance.HasKey)
        {
            StartCoroutine(OpenSequence());
        }
    }

    private IEnumerator OpenSequence()
    {
        hasOpened = true;

        if (lockAnimator != null)
        {
            lockAnimator.Play(lockStateName);
            yield return null;
            var lockStateInfo = lockAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(lockStateInfo.length);
        }

        if (doorAnimator != null)
        {
            doorAnimator.Play(openStateName);
            yield return null;
            var doorStateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(doorStateInfo.length);
        }

        // === LEVEL COMPLETE SOUND ===
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(AudioManager.Instance.levelCompleteSound);
        else
            Debug.LogWarning("AudioManager not found for level complete sound.");

        TeleportToNextLevel();
    }

    private void TeleportToNextLevel()
    {
        Debug.Log("Sequence complete! Transitioning to next level...");
        
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("DoorScript: Next Scene Name is empty! Assign it in the Inspector.");
            return;
        }

        if (LevelTransitioner.Instance != null)
        {
            LevelTransitioner.Instance.TransitionToLevel(nextSceneName);
            return;
        }

        // Fallback if the transitioner isn't present in the scene.
        SceneManager.LoadScene(nextSceneName);
    }
}