using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public string p1d;
    public string p2d;
    public int gameId;

    public bool gameIsOver = false;
    public string winner;

    public bool isp1turn;
    public bool islocalturn;

    public TMP_Text p1Text;
    public TMP_Text GameIDText;
    public TMP_Text p2Text;

    public BoardController boardController;

    public string gameHistory = "";

    List<int[]> boardstate = new List<int[]>(13);
    int[] height = new int[13];

    public void CalculateBoard()
    {

        boardstate = new List<int[]>(13);
        height = new int[13];
        for (int i = 0; i < 13; i++)
        {
            boardstate.Add(new int[12]);
        }


        int isp1 = (DBManager.username == p1d) ? 1 : -1;
        Debug.Log(gameHistory);
        foreach (char ch in gameHistory)
        {
            boardstate[int.Parse(ch.ToString()) + 3][height[int.Parse(ch.ToString())+3] + 3] = isp1;
            height[int.Parse(ch.ToString())+3]++;
            isp1 *= -1;
        }

        //for (int i = 8; i >= 3; i--)
        //{
        //    string debugstring = "";

        //    for (int j = 3; j < 10; j++)
        //    {
        //        debugstring += boardstate[j][i] + " ";
        //    }
        //    Debug.Log(debugstring);
        //}
    }

    public void ImClicked()
    {
        boardController.SelectMe(this, false);
    }

    public void UpdateObjects()
    {
        p1Text.text = p1d;
        p2Text.text = p2d;
        GameIDText.text = "#" + gameId.ToString();
        CheckForLocalTurn();
        CalculateBoard();
        CheckForGameOver();
        if (gameIsOver)
            gameObject.GetComponent<Image>().color = new Color(200, 200, 200);
    }

    public void CheckForLocalTurn()
    {
        if(gameHistory == null)
        {
            gameHistory = "";
        }
        isp1turn = (gameHistory.Length % 2 == 0);
        if (!gameIsOver)
        {
            if (isp1turn && p1d == DBManager.username || !isp1turn && p2d == DBManager.username)
            {
                islocalturn = true;
                gameObject.GetComponent<Image>().color = new Color(0, 155, 0);

            }
            else
            {
                islocalturn = false;
                gameObject.GetComponent<Image>().color = new Color(155, 0, 0);
            }
            boardController.sendMoveButton.SetActive(islocalturn);
        }

    }

    public int CheckForGameOver()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (CheckForMatrixPosForWin(i,j) != 0)
                {
                    return CheckForMatrixPosForWin(i, j);
                }
            }
        }
        return 0;
    }

    int CheckForMatrixPosForWin(int x, int y)
    {
        int[] dirs = new int[9];
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (j == 0 && i == 0)
                    continue;
                if (boardstate[x + 3][y + 3] == 0)
                    continue;
                int incr = 1;


                while(true)
                {
                    if (incr == 4)
                        break;
                    if (boardstate[3+x+incr*i][3+y+incr*j] != boardstate[x+3][y+3])
                    {
                        break;
                    }
                    incr++;
                    dirs[3 * (i + 1) + (j + 1)]++;
                }
                //Debug.Log("HELLO");
            }
        }

        if (dirs[0]+dirs[6] > 2 || dirs[1] + dirs[7] > 2 || dirs[2] + dirs[8] > 2 || dirs[3] + dirs[5] > 2) // if someone has won
        {
            Debug.Log("THIS IS WIN" + x.ToString() + y.ToString());
            gameIsOver = true;
            if(boardstate[x + 3][y + 3] == 1)
            {
                winner = "You, " + DBManager.username;
            }
            else
            {
                if(DBManager.username != p1d)
                {
                    winner = p1d;
                }
                else
                {
                    winner = p2d;
                }
            }
            return boardstate[x + 3][y + 3];
        }
        return 0;
    }

}
