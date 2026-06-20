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

    // Existing method (unchanged)
    private IEnumerator TransitionSequence(string sceneName)
    {
        // 1. START THE 'TO LEVEL' SECTOR
        if (toLevelCanvas != null && toLevelAnimator != null)
        {
            toLevelCanvas.SetActive(true);
            toLevelAnimator.Play(toLevelStateName);
            
            yield return null;
            var stateInfo = toLevelAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
        }

        // 2. LOAD THE NEW SCENE
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null; 
        }

        // Clean up
        if (toLevelCanvas != null) toLevelCanvas.SetActive(false);

        // 3. START THE 'FROM LEVEL' SECTOR
        if (fromLevelCanvas != null && fromLevelAnimator != null)
        {
            fromLevelCanvas.SetActive(true);
            fromLevelAnimator.Play(fromLevelStateName);
            
            yield return null;
            var stateInfo = fromLevelAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
            
            fromLevelCanvas.SetActive(false);
        }
    }

    // ==================== NEW METHOD FOR DEATH BORDER ====================
    public void ReloadCurrentLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(ReloadCurrentLevelSequence(currentSceneName));
    }

    private IEnumerator ReloadCurrentLevelSequence(string sceneName)
    {
        // Same transition logic as normal level change
        yield return StartCoroutine(TransitionSequence(sceneName));
    }
}