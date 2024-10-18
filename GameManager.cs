using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool IsX = false, IsO = false;
    [SerializeField] private TMP_Text inform;
    [SerializeField] public GameObject[] objectList;
    #endregion

    #region Classes
    private Board board;
    #endregion 

    #region Variables
    private int move, xs, os, xc, oc, xw, ow, dw;
    private List<int> xMoves, oMoves;
    private bool isStart;
    Color32 red, blue, orange;
    #endregion

    private void Start()
    {
        xMoves = new List<int>();
        oMoves = new List<int>();

        red = new Color32(255, 132, 117, 255);
        blue = new Color32(143, 255, 250, 255);
        orange = new Color32(255, 237, 143, 255);

        xw = PlayerPrefs.GetInt("WX", 0);
        ow = PlayerPrefs.GetInt("WX", 0);
        dw = PlayerPrefs.GetInt("WD", 0);

        inform.text = "";
        ButtonManager(objectList, 0);

        isStart = true;
        board = GetComponent<Board>();
        board.InitIiles();
        InitUI(InitMove());
    }

    public void CheckMove(Tile tile)
    {
        Valid(tile);

        if (CheckWin())
        {

            board.Inactive();
            if (IsX)
            {
                inform.text = "X Won!";
                inform.color = red;
                PlayerPrefs.SetInt("WX", xw + 1);
            }
            else
            {
                inform.text = "O Won!";
                inform.color = blue;
                PlayerPrefs.SetInt("WO", ow + 1);
            }
            return;
        }

        if (CheckDraw())
        {
            board.Inactive();
            inform.text = "Draw!";
            inform.color = orange;
            PlayerPrefs.SetInt("WD", dw + 1);
            return;
        }

        tile.GetComponent<Button>().interactable = ChangeMove();
        tile.IsClicked = true;
    }

    private int InitMove()
    {
        var random = new System.Random();
        move = random.Next(2);
        return move;
    }

    private void InitUI(int i)
    {
        inform.text = $"{UpdateUI(i)} Move";

        IsX = i == 0;
        IsO = i == 1;

        if (i > 1) Menu();
    }

    private string UpdateUI(int i)
    {
        return (i == 0) ? "X" : (i == 1) ? "O" : "";
    }

    private bool ChangeMove()
    {
        if (move == 0)
        {
            move = 1;
            InitUI(1);
        }
        else if (move == 1)
        {
            move = 0;
            InitUI(0);
        }
        else InitUI(2);

        return false;
    }

    private void Valid(Tile tile)
    {
        int i = (tile != null) ? tile.Id : 0;

        if (!tile.IsClicked)
        {
            if (IsX)
            {
                tile.GetComponentInChildren<TMP_Text>().text = UpdateUI(0);
                tile.GetComponentInChildren<TMP_Text>().color = red;

                xMoves.Add(i);
                xs = (xMoves.Count == 1) ? xMoves[0] : xs;
                xc = (xMoves.Count > 1) ? xMoves[xMoves.Count - 1] : xc;
            }
            else if (IsO)
            {
                tile.GetComponentInChildren<TMP_Text>().text = UpdateUI(1);
                tile.GetComponentInChildren<TMP_Text>().color = blue;

                oMoves.Add(i);
                os = (oMoves.Count == 1) ? oMoves[0] : os;
                oc = (oMoves.Count > 1) ? oMoves[oMoves.Count - 1] : oc;
            }
        }

        if (xMoves.Count > 1 && oMoves.Count > 1) isStart = false;
    }

    private bool CheckWin()
    {
        int[,] winPatterns = new int[,]
        {
            {0, 1, 2}, // Горизонтальные
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6}, // Вертикальные
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8}, // Диагонали
            {2, 4, 6}
        };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (xMoves.Contains(a) && xMoves.Contains(b) && xMoves.Contains(c)) return true; // X выиграл

            if (oMoves.Contains(a) && oMoves.Contains(b) && oMoves.Contains(c)) return true; // O выиграл
        }

        return false;
    }

    private void ButtonManager(GameObject[] b, int i)
    {
        b[0].GetComponent<Button>().onClick.AddListener(ShowGameSpace);
        b[0].GetComponentInChildren<TMP_Text>().text = "Play!";

        b[1].GetComponent<GameSpace>().Restart.onClick.AddListener(Play);
        b[1].GetComponent<GameSpace>().Back.onClick.AddListener(Menu);

        b[1].GetComponent<GameSpace>().Restart.GetComponentInChildren<TMP_Text>().text = "Restart";
        b[1].GetComponent<GameSpace>().Back.GetComponentInChildren<TMP_Text>().text = "Back";

        foreach (var x in b)
            x.SetActive(false);

        b[i].SetActive(true);
    }

    private bool CheckDraw()
    {
        return xMoves.Count + oMoves.Count == 9 && !CheckWin();
    }

    private void ShowGameSpace() => ButtonManager(objectList, 1);

    public void Play() => StartCoroutine(Load(1));

    public void Menu() => StartCoroutine(Load(0));

    public IEnumerator Load(int i)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(i);
        if (operation.isDone) yield return new WaitForEndOfFrame();
    }
}