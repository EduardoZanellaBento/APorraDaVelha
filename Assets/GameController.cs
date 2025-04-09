using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameController : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    public List<Image> cellImages;

    public TMP_Text labelX;
    public TMP_Text labelO;
    public TMP_Text endText;

    private string[] board = new string[9];
    private bool turnX = true;
    private bool gameOver = false;

    public void OnCellClicked(int index)
    {
        if (board[index] != null || gameOver)
            return;

        board[index] = turnX ? "X" : "O";
        cellImages[index].sprite = turnX ? xSprite : oSprite;

        Button button = cellImages[index].transform.parent.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        if (CheckWin(board[index]))
        {
            endText.text = $"Vitória do jogador {board[index]}!";
            gameOver = true;
            HighlightWinner(board[index]);
            return;
        }
        else if (IsBoardFull())
        {
            endText.text = "Empate!";
            gameOver = true;
            return;
        }

        turnX = !turnX;
        UpdateTurnUI();
    }

    private void UpdateTurnUI()
    {
        if (turnX)
        {
            labelX.text = "<--";
            labelO.text = "";
        }
        else
        {
            labelX.text = "";
            labelO.text = "<--";
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
            if (board[winCombos[i,0]] == player &&
                board[winCombos[i,1]] == player &&
                board[winCombos[i,2]] == player)
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

    private void HighlightWinner(string player)
    {
        // trocar de cor ou efeitos visuais futuramente.
    }

    private void Start()
    {
        UpdateTurnUI();
    }
}
