using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button easyButton, mediumButton, hardButton;
    public Button vsAIButton, vsPlayerButton;
    public Button startButton;

    private void Start()
    {
        easyButton.onClick.AddListener(() => SetDifficulty(GameSettings.Difficulty.Easy));
        mediumButton.onClick.AddListener(() => SetDifficulty(GameSettings.Difficulty.Medium));
        hardButton.onClick.AddListener(() => SetDifficulty(GameSettings.Difficulty.Hard));

        vsAIButton.onClick.AddListener(() => SetMode(true));
        vsPlayerButton.onClick.AddListener(() => SetMode(false));

        startButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));

        // Configura visual inicial
        UpdateDifficultyButtons();
        UpdateModeButtons();
    }

    void SetDifficulty(GameSettings.Difficulty difficulty)
    {
        GameSettings.Instance.SelectedDifficulty = difficulty;
        UpdateDifficultyButtons();
    }

    void SetMode(bool isAI)
    {
        GameSettings.Instance.PlayAgainstAI = isAI;
        UpdateModeButtons();
    }

    void UpdateDifficultyButtons()
    {
        HighlightButton(easyButton, GameSettings.Instance.SelectedDifficulty == GameSettings.Difficulty.Easy);
        HighlightButton(mediumButton, GameSettings.Instance.SelectedDifficulty == GameSettings.Difficulty.Medium);
        HighlightButton(hardButton, GameSettings.Instance.SelectedDifficulty == GameSettings.Difficulty.Hard);
    }

    void UpdateModeButtons()
    {
        HighlightButton(vsAIButton, GameSettings.Instance.PlayAgainstAI);
        HighlightButton(vsPlayerButton, !GameSettings.Instance.PlayAgainstAI);
    }

    void HighlightButton(Button button, bool highlight)
    {
        var colors = button.colors;
        colors.normalColor = highlight ? Color.green : Color.white;
        button.colors = colors;
    }
}
