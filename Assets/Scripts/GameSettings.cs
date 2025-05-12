using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public enum Difficulty { None, Easy, Medium, Hard }
    public Difficulty SelectedDifficulty = Difficulty.None;
    public bool? PlayAgainstAI = null;

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
