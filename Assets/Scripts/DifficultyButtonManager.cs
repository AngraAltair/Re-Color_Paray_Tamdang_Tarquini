using UnityEngine;

public class DifficultyButtonManager : MonoBehaviour
{
    public static DifficultyButtonManager Instance { get; private set; }

    [Header("Difficulty Buttons")]
    [SerializeField] private DifficultyButton easyButton;
    [SerializeField] private DifficultyButton mediumButton;
    [SerializeField] private DifficultyButton hardButton;

    private DifficultyButton currentSelected;

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
        int lastDifficulty = PlayerPrefs.GetInt("LastDifficulty", 0);

        if (lastDifficulty == 1)      SelectButton(mediumButton);
        else if (lastDifficulty == 2) SelectButton(hardButton);
        else                          SelectButton(easyButton);
    }

    public void SelectButton(DifficultyButton selected)
    {
        if (currentSelected != null)
            currentSelected.Deselect();

        currentSelected = selected;
        currentSelected.Select();

        // Also notify LevelSelector to slide the right group
        NotifyLevelSelector(selected);
    }

    private void NotifyLevelSelector(DifficultyButton selected)
    {
        LevelSelector levelSelector = FindObjectOfType<LevelSelector>();
        if (levelSelector == null) return;

        if (selected == easyButton)       levelSelector.OnEasySelected();
        else if (selected == mediumButton) levelSelector.OnMediumSelected();
        else if (selected == hardButton)   levelSelector.OnHardSelected();
    }
}