using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty SelectedDifficulty = Difficulty.Medium;
    public bool PlayAgainstAI = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém entre cenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicatas
        }
    }
}
