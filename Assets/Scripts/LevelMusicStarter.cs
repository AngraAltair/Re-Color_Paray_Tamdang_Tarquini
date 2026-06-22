using UnityEngine;

public class LevelMusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;

    private void Start()
    {
        if (MusicManager.Instance == null || levelMusic == null) return;

        if (MusicManager.Instance.CurrentClip != levelMusic)
        {
            MusicManager.Instance.PlayMusic(levelMusic);
        }
    }
}
