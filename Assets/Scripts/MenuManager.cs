using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button easyButton, mediumButton, hardButton;
    public Button vsAIButton, vsPlayerButton;
    public Button startButton;

    public GameObject difficultyPanel; // <- atribua no Inspector (Painel dos 3 botões de dificuldade)

    private void Start()
    {
        GameSettings.Instance.PlayAgainstAI = null;
        GameSettings.Instance.SelectedDifficulty = GameSettings.Difficulty.None;    
        easyButton.onClick.AddListener(() => SetDifficulty(GameSettings.Difficulty.Easy));
        mediumButton.onClick.AddListener(() => SetDifficulty(GameSettings.Difficulty.Medium));
        hardButton.onClick.AddListener(() => SetDifficulty(GameSettings.Difficulty.Hard));

        vsAIButton.onClick.AddListener(() => SetMode(true));
        vsPlayerButton.onClick.AddListener(() => SetMode(false));

        startButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));

        difficultyPanel.SetActive(false); // oculta no início
        startButton.gameObject.SetActive(false); // desativa no início

        UpdateUI();
    }

    void SetDifficulty(GameSettings.Difficulty difficulty)
    {
        GameSettings.Instance.SelectedDifficulty = difficulty;
        UpdateDifficultyButtons();
        UpdateStartButton();
    }

    void SetMode(bool isAI)
    {
        GameSettings.Instance.PlayAgainstAI = isAI;
        difficultyPanel.SetActive(isAI); // mostra dificuldade só se for contra IA
        GameSettings.Instance.SelectedDifficulty = GameSettings.Difficulty.None; // reset

        UpdateModeButtons();
        UpdateDifficultyButtons();
        UpdateStartButton();
    }

    void UpdateStartButton()
    {
        if (GameSettings.Instance.PlayAgainstAI == null)
        {
            startButton.gameObject.SetActive(false);
            return;
        }

        if (GameSettings.Instance.PlayAgainstAI == false)
        {
            startButton.gameObject.SetActive(true);
        }
        else
        {
            bool difficultyChosen = GameSettings.Instance.SelectedDifficulty != GameSettings.Difficulty.None;
            startButton.gameObject.SetActive(difficultyChosen);
        }
    }

    void UpdateDifficultyButtons()
    {
        var difficulty = GameSettings.Instance.SelectedDifficulty;

        HighlightButton(easyButton, difficulty == GameSettings.Difficulty.Easy);
        HighlightButton(mediumButton, difficulty == GameSettings.Difficulty.Medium);
        HighlightButton(hardButton, difficulty == GameSettings.Difficulty.Hard);
    }

    void UpdateModeButtons()
    {
        bool? playAgainstAI = GameSettings.Instance.PlayAgainstAI;

        HighlightButton(vsAIButton, playAgainstAI == true);
        HighlightButton(vsPlayerButton, playAgainstAI == false);
    }

    void HighlightButton(Button button, bool highlight)
    {
        var colors = button.colors;
        colors.normalColor = highlight ? Color.green : Color.white;
        button.colors = colors;
    }

    void UpdateUI()
    {
        UpdateModeButtons();
        UpdateDifficultyButtons();
        UpdateStartButton();
    }
}
