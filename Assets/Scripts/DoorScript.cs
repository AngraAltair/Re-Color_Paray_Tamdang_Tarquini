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
        // Lock the sequence so it can't be triggered multiple times
        hasOpened = true;

        // 1. Play the Lock animation first
        if (lockAnimator != null)
        {
            lockAnimator.Play(lockStateName);
            
            // Wait a brief moment for the animator to switch states, then wait for its duration
            yield return null; 
            var lockStateInfo = lockAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(lockStateInfo.length);
        }

        // 2. Lock is done! Now play the DoorToOpen animation
        if (doorAnimator != null)
        {
            doorAnimator.Play(openStateName);
            
            // Wait for the door opening animation to completely finish
            yield return null;
            var doorStateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(doorStateInfo.length);
        }

        // 3. Both animations are complete. Teleport/Load the next level!
        TeleportToNextLevel();
    }

    private void TeleportToNextLevel()
    {
        Debug.Log("Sequence complete! Transitioning to next level...");
        
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("DoorScript: Next Scene Name is empty! Assign it in the Inspector.");
        }
    }
}