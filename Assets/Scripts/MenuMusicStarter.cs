using UnityEngine;

public class MenuMusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip menuMusic;

    private void Start()
    {
        if (MusicManager.Instance != null && menuMusic != null)
        {
            MusicManager.Instance.PlayIfNotPlaying(menuMusic);
        }
    }
}
