using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Object References")]
    // [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject player;
    private SpriteRenderer spriteRenderer;

    [Header("Inventory")]
    [SerializeField] private bool hasKey = false;

    [Header("Player State")]
    [SerializeField] private bool isAlive = true;

    public bool HasKey => hasKey;
    public bool IsAlive => isAlive;

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
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    public void CollectKey()
    {
        if (hasKey)
            return;

        hasKey = true;
        Debug.Log("PlayerManager: Key collected.");
    }
}