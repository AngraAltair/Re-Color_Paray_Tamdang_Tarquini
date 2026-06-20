using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Player Sounds")]
    public AudioClip jumpSound;
    public AudioClip crayonCollectSound;
    public AudioClip deathInstakillSound;
    public AudioClip deathBorderSound;

    [Header("Level Sounds")]
    public AudioClip levelCompleteSound;

    [Header("Object Sounds")]
    public AudioClip yellowBlueCombineSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Make this object persist across scene loads
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
