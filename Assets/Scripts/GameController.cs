using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameController : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    public List<Image> symbolImages; // imagem interna (X ou O)

    public TMP_Text endText;
    public TMP_Text scoreTextX;
    public TMP_Text scoreTextO;

    public Button restartButton;

    private string[] board = new string[9];
    private bool turnX = true;
    private bool gameOver = false;

    private int scoreX = 0;
    private int scoreO = 0;
    private int round = 1;

    public bool isVsAI = true;

    private GameSettings.Difficulty aiDifficulty = GameSettings.Difficulty.Medium;

    public void OnCellClicked(int index)
    {
        if (board[index] != null || gameOver)
            return;

        board[index] = turnX ? "X" : "O";
        symbolImages[index].sprite = turnX ? xSprite : oSprite;

        Button button = symbolImages[index].transform.parent.GetComponent<Button>();
        if (button != null)
            button.interactable = false;

        if (CheckWin(board[index]))
        {
            endText.text = $"Vitória do jogador {board[index]}!";
            gameOver = true;

            if (board[index] == "X") scoreX++;
            else scoreO++;

            UpdateScoreUI();

            if (scoreX == 2 || scoreO == 2)
            {
                endText.text += $"\nJogador {board[index]} venceu a melhor de 3!";
                restartButton.gameObject.SetActive(true);
            }
            else
            {
                Invoke(nameof(StartNextRound), 2f);
            }

            return;
        }
        else if (IsBoardFull())
        {
            endText.text = "Empate!";
            gameOver = true;
            Invoke(nameof(StartNextRound), 2f);
            return;
        }

        turnX = !turnX;

        if (!gameOver && !turnX && isVsAI)
        {
            Invoke(nameof(PlayAI), 0.5f);
        }
    }

    private void PlayAI()
    {
        int move = -1;

        switch (aiDifficulty)
        {
            case GameSettings.Difficulty.Easy:
                move = PlayRandomMove();
                break;
            case GameSettings.Difficulty.Medium:
                move = PlaySmartMove();
                break;
            case GameSettings.Difficulty.Hard:
                move = PlayBestMove();
                break;
        }

        if (move != -1)
        {
            OnCellClicked(move);
        }
    }

    private int PlayRandomMove()
    {
        List<int> available = new List<int>();
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == null)
                available.Add(i);
        }

        if (available.Count == 0) return -1;

        return available[Random.Range(0, available.Count)];
    }

    private int PlaySmartMove()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == null)
            {
                board[i] = "O";
                if (CheckWin("O"))
                {
                    board[i] = null;
                    return i;
                }
                board[i] = null;
            }
        }

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == null)
            {
                board[i] = "X";
                if (CheckWin("X"))
                {
                    board[i] = null;
                    return i;
                }
                board[i] = null;
            }
        }

        return PlayRandomMove();
    }

    private int PlayBestMove()
    {
        int bestScore = int.MinValue;
        int move = -1;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == null)
            {
                board[i] = "O";
                int score = Minimax(board, false);
                board[i] = null;

                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        return move;
    }

    private int Minimax(string[] boardState, bool isMaximizing)
    {
        if (CheckWin("O")) return 1;
        if (CheckWin("X")) return -1;
        if (IsBoardFull()) return 0;

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == null)
            {
                boardState[i] = isMaximizing ? "O" : "X";
                int score = Minimax(boardState, !isMaximizing);
                boardState[i] = null;

                if (isMaximizing)
                    bestScore = Mathf.Max(score, bestScore);
                else
                    bestScore = Mathf.Min(score, bestScore);
            }
        }

        return bestScore;
    }

    private void StartNextRound()
    {
        if (scoreX == 2)
        {
            endText.text = $"Jogador X venceu a melhor de 3!";
            restartButton.gameObject.SetActive(true);
            return;
        }
        else if (scoreO == 2)
        {
            endText.text = $"Jogador O venceu a melhor de 3!";
            restartButton.gameObject.SetActive(true);
            return;
        }

        round++;
        ResetBoardOnly();
        endText.text = $"Rodada {round}";

        if (!turnX && isVsAI)
            Invoke(nameof(PlayAI), 0.5f);
    }

    private void ResetBoardOnly()
    {
        board = new string[9];
        gameOver = false;

        for (int i = 0; i < symbolImages.Count; i++)
        {
            symbolImages[i].sprite = null;
            Button button = symbolImages[i].transform.parent.GetComponent<Button>();
            if (button != null)
                button.interactable = true;
        }
    }

    private bool CheckWin(string player)
    {
        int[,] winCombos = new int[,] {
            {0,1,2}, {3,4,5}, {6,7,8},
            {0,3,6}, {1,4,7}, {2,5,8},
            {0,4,8}, {2,4,6}
        };

        for (int i = 0; i < winCombos.GetLength(0); i++)
        {
            if (board[winCombos[i, 0]] == player &&
                board[winCombos[i, 1]] == player &&
                board[winCombos[i, 2]] == player)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsBoardFull()
    {
        foreach (string cell in board)
        {
            if (cell == null) return false;
        }
        return true;
    }

    private void UpdateScoreUI()
    {
        scoreTextX.text = $"X: {scoreX}";
        scoreTextO.text = $"O: {scoreO}";
    }

    public void RestartFullGame()
    {
        scoreX = 0;
        scoreO = 0;
        round = 1;
        turnX = true;
        gameOver = false;

        ResetBoardOnly();
        UpdateScoreUI();
        endText.text = "";
        restartButton.gameObject.SetActive(true);

        if (!turnX && GameSettings.Instance.PlayAgainstAI == true)
            Invoke(nameof(PlayAI), 0.5f);
    }

    private void Start()
    {
        if (GameSettings.Instance != null)
        {
            isVsAI = GameSettings.Instance.PlayAgainstAI == true;
            aiDifficulty = GameSettings.Instance.SelectedDifficulty;
        }
        else
        {
            isVsAI = true;
            aiDifficulty = GameSettings.Difficulty.Medium;
        }

        UpdateScoreUI();
        restartButton.gameObject.SetActive(true);

        if (!turnX && GameSettings.Instance.PlayAgainstAI == true)
            Invoke(nameof(PlayAI), 0.5f);
    }

    private void HighlightWinner(string player)
    {
        // efeito visual opcional
    }
}
