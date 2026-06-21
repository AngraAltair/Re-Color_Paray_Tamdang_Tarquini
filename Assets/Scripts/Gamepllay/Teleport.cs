using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum Mode
    {
        Spawner,
        Collector
    }

    [Header("Mode")]
    public Mode currentMode = Mode.Spawner;

    [Header("Spawn Settings (Spawner only)")]
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2f;

    [Header("Collector Settings (Collector only)")]
    public ItemManager spawnerReference;   // Optional now

    [Header("General")]
    public bool autoStart = true;
    public string collectibleTag = "Collectible";   // Tag for yellow objects

    private bool isSpawning = false;

    private void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;

        if (currentMode == Mode.Spawner && autoStart)
            StartSpawning();
    }

    private void StartSpawning()
    {
        if (currentMode != Mode.Spawner || isSpawning) return;
        
        isSpawning = true;
        InvokeRepeating(nameof(SpawnItem), 2f, spawnDelay);
    }

    private void SpawnItem()
    {
        if (currentMode != Mode.Spawner || itemPrefab == null) return;
        Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // Collector logic
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentMode != Mode.Collector) return;

        if (other.CompareTag(collectibleTag))
        {
            Destroy(other.gameObject);

            if (spawnerReference != null && other.transform.parent == spawnerReference.transform)
            {
                spawnerReference.NotifyItemCollected();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentMode != Mode.Collector) return;

        if (collision.gameObject.CompareTag(collectibleTag))
        {
            Destroy(collision.gameObject);

            if (spawnerReference != null && collision.transform.parent == spawnerReference.transform)
            {
                spawnerReference.NotifyItemCollected();
            }
        }
    }


    public void NotifyItemCollected()
    {
        
    }
}
