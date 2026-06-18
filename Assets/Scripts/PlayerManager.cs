using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Inventory")]
    [SerializeField] private bool hasKey = false;

    public bool HasKey => hasKey;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CollectKey()
    {
        if (hasKey)
            return;

        hasKey = true;
        Debug.Log("PlayerManager: Key collected.");
    }
}