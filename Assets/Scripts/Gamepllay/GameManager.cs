using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player References")]
    [SerializeField] private GameObject player;           // Assign in inspector
    [SerializeField] private Transform playerSpawnPoint;  // Assign in inspector

    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject audioPanel;

    [SerializeField] private Image pauseIcon;       // UI Image component
    [SerializeField] private Sprite pausedSprite;   // Sprite to show when paused
    [SerializeField] private Sprite resumedSprite;  // Sprite to show when resumed

    [Header("Player State")]
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;
    private Coroutine deathCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
    }

    private void Start()
    {
        if (player != null)
        {
            spriteRenderer = player.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Player reference is missing in GameManager!");
        }
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.R))
    {
        RestartLevel();
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
        TogglePausePanel();
    }
}


    // ==================== DEATH & RESPAWN ====================

    public void KillPlayer()
    {
        if (!isAlive) return;

        isAlive = false;

        if (spriteRenderer != null) spriteRenderer.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        GrayMovement controller = GetComponent<GrayMovement>();
        if (controller != null) controller.enabled = false;

        Debug.Log("Player killed.");
    }

    public void RespawnPlayer()
    {
        if (isAlive) return;

        isAlive = true;
        if (spriteRenderer != null) spriteRenderer.enabled = true;

        if (player != null && playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }

        Debug.Log("Player respawned.");
    }

    private IEnumerator RespawnSequence()
    {
        KillPlayer();
        yield return new WaitForSeconds(.1f);
        RespawnPlayer();
    }

    public void KillAndRespawnPlayer(bool resetProgress = false)
    {
        if (deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine);
        }

        if (resetProgress)
        {
            RestartLevel();
        }
        else
        {
            deathCoroutine = StartCoroutine(RespawnSequence());
        }
    }

    // ==================== LEVEL RESTART ====================

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevelWithTransition()
    {
        if (LevelTransitioner.Instance != null)
        {
            LevelTransitioner.Instance.ReloadCurrentLevel();
        }
        else
        {
            Debug.LogWarning("LevelTransitioner not found. Falling back to direct reload.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // ==================== UI ====================

    public void TogglePausePanel()
    {
        if (pausePanel != null)
        {
            bool active = !pausePanel.activeSelf;
            pausePanel.SetActive(active);
            Time.timeScale = active ? 0f : 1f;

            // Swap sprite
            if (pauseIcon != null)
                pauseIcon.sprite = active ? pausedSprite : resumedSprite;
        }

        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }
    }

    public void ToggleAudioPanel()
    {
        if (audioPanel != null)
        {
            audioPanel.SetActive(!audioPanel.activeSelf);
        }
    }

    public void ReturnToLevelSelect()
    {
        Time.timeScale = 1f;

        if (LevelTransitioner.Instance != null)
        {
            PlayerPrefs.SetInt("LastDifficulty", GetDifficultyFromScene());
            PlayerPrefs.Save();
            SceneManager.LoadScene("LevelSelect");
            LevelTransitioner.Instance.TransitionToLevel("LevelSelection"); 
        }
        else
        {
            PlayerPrefs.SetInt("LastDifficulty", GetDifficultyFromScene());
            PlayerPrefs.Save();
            SceneManager.LoadScene("LevelSelection");
        }
    }

        private int GetDifficultyFromScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("2-")) return 1;
        if (sceneName.StartsWith("3-")) return 2;
        return 0;                                  
    }
}