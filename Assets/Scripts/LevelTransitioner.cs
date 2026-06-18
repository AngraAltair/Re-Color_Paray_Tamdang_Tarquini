using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour
{
    public static LevelTransitioner Instance { get; private set; }

    [Header("Canvas & Animator 1: Moving TO a Level")]
    [Tooltip("The Canvas GameObject that plays the rainbow entry sequence")]
    [SerializeField] private GameObject toLevelCanvas;
    [Tooltip("The Animator on the ToLevel Canvas")]
    [SerializeField] private Animator toLevelAnimator;
    [SerializeField] private string toLevelStateName = "ToLevel";

    [Header("Canvas & Animator 2: Loading FROM a Level")]
    [Tooltip("The Canvas GameObject that plays the rainbow exit sequence")]
    [SerializeField] private GameObject fromLevelCanvas;
    [Tooltip("The Animator on the FromLevel Canvas")]
    [SerializeField] private Animator fromLevelAnimator;
    [SerializeField] private string fromLevelStateName = "FromLevel";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (toLevelCanvas != null) toLevelCanvas.SetActive(false);
        
        if (fromLevelCanvas != null) fromLevelCanvas.SetActive(false);
    }

    public void TransitionToLevel(string sceneName)
    {
        StartCoroutine(TransitionSequence(sceneName));
    }

    private IEnumerator TransitionSequence(string sceneName)
    {
        // 1. START THE 'TO LEVEL' SECTOR (Enter the Rainbow)
        if (toLevelCanvas != null && toLevelAnimator != null)
        {
            toLevelCanvas.SetActive(true); // Turn the canvas on so it can render
            toLevelAnimator.Play(toLevelStateName);
            
            // Wait for the 7 rainbow panels to completely cover the screen
            yield return null;
            var stateInfo = toLevelAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
        }

        // 2. LOAD THE NEW SCENE IN THE BACKGROUND
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null; 
        }

        // Clean up the first canvas now that the new scene is loaded
        if (toLevelCanvas != null) toLevelCanvas.SetActive(false);

        // 3. START THE 'FROM LEVEL' SECTOR (Exit the Rainbow)
        if (fromLevelCanvas != null && fromLevelAnimator != null)
        {
            fromLevelCanvas.SetActive(true); // Turn it on
            fromLevelAnimator.Play(fromLevelStateName);
            
            // Wait for the 7 panels to clear away revealing the new level
            yield return null;
            var stateInfo = fromLevelAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
            
            fromLevelCanvas.SetActive(false); // Shut it off so it doesn't block player clicks!
        }
    }
}