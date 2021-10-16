using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardController : MonoBehaviour
{
    public GameData currentGame;

    public GameObject placeHolderPiece;
    int placeHolderIndex = 3;

    public GameObject errorText;

    public GameObject winText;

    public GameObject piecePrefab;
    List<GameObject> pieces = new List<GameObject>();
    int[] height = new int[7];

    public GameObject sendMoveButton;
    public TMP_Text P1TEXT;
    public TMP_Text P2TEXT;
    public TMP_Text GameId;
    public GameObject waitingForOpponent;

    public void SelectMe(GameData data, bool comand)
    {
        StopCoroutine("spawnAllPieces");
        if (data == currentGame && !comand)
            return;
        placeHolderPiece.SetActive(false);
        waitingForOpponent.SetActive(false);
        //Debug.Log("TESTTSENTESNETNS");
        currentGame = data;
        height = new int[7];
        foreach(GameObject piece in pieces)
        {
            Destroy(piece);
        }
        pieces = new List<GameObject>();
        if(currentGame.gameHistory != null && currentGame.gameHistory != "")
        {
            StartCoroutine("spawnAllPieces");
        }
        else
        {
            currentGame.CheckForLocalTurn();
            if (currentGame.islocalturn && !currentGame.gameIsOver)
            {
                placeHolderPiece.SetActive(true);
                waitingForOpponent.SetActive(false);
                placeHolderIndex = 3;
                placeHolderPiece.transform.localPosition = new Vector3(-3.75f + 1.25f * placeHolderIndex, -2.5f + 1.25f * height[placeHolderIndex], 0);
                placeHolderPiece.GetComponent<SpriteRenderer>().color = (currentGame.p1d == DBManager.username) ? new Color(0, 255, 0, 0.5f) : new Color(0, 0, 255, 0.5f);
            }
            else
            {
                waitingForOpponent.SetActive(true);
            }
        }
        P1TEXT.text = currentGame.p1d;
        P2TEXT.text = currentGame.p2d;
        GameId.text = "#" + currentGame.gameId;

        if(currentGame.gameIsOver)
        {
            placeHolderPiece.SetActive(false);
            winText.SetActive(true);
            sendMoveButton.SetActive(false);
            winText.GetComponent<TMP_Text>().text = currentGame.winner + " won this game";
        }
        else
        {
            winText.SetActive(false);
        }
    }

    private void Start()
    {
        currentGame = new GameData();
    }

    IEnumerator spawnAllPieces()
    {
        bool isp1 = true;
        foreach(char move in currentGame.gameHistory)
        {
            int intmove = int.Parse(move.ToString());
            GameObject spawnedPiece = Instantiate(piecePrefab, transform);
            pieces.Add(spawnedPiece);
            spawnedPiece.transform.localPosition = new Vector3(-3.75f + 1.25f*intmove, -2.5f + 1.25f*height[intmove], 0);
            height[intmove]++;
            spawnedPiece.GetComponent<SpriteRenderer>().color = isp1 ? new Color(0, 255, 0) : new Color(0, 0, 255);
            isp1 = !isp1;
            yield return new WaitForSeconds(0.075f);
        }
        currentGame.CheckForLocalTurn();
        if(currentGame.islocalturn && !currentGame.gameIsOver)
        {
            placeHolderPiece.SetActive(true);
            placeHolderIndex = 3;
            placeHolderPiece.transform.localPosition = new Vector3(-3.75f + 1.25f * placeHolderIndex, -2.5f + 1.25f * height[placeHolderIndex], 0);
            placeHolderPiece.GetComponent<SpriteRenderer>().color = (currentGame.p1d == DBManager.username) ? new Color(0, 255, 0, 0.5f) : new Color(0, 0, 255, 0.5f);
        }
        else
        {
            if(!currentGame.gameIsOver)
                waitingForOpponent.SetActive(true);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(placeHolderIndex != 0)
                placeHolderIndex--;
            placeHolderPiece.transform.localPosition = new Vector3(-3.75f + 1.25f * placeHolderIndex, -2.5f + 1.25f * height[placeHolderIndex], 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (placeHolderIndex != 6)
                placeHolderIndex++;
            placeHolderPiece.transform.localPosition = new Vector3(-3.75f + 1.25f * placeHolderIndex, -2.5f + 1.25f * height[placeHolderIndex], 0);
        }
    }

    public void SendMove()
    {
        if(currentGame.islocalturn)
        {
            currentGame.gameHistory += placeHolderIndex;
            StartCoroutine(PostMove());
            foreach (GameObject piece in pieces)
            {
                Destroy(piece);
            }
            pieces = new List<GameObject>();
            height = new int[7];
            placeHolderPiece.SetActive(false);
            waitingForOpponent.SetActive(true);
            StartCoroutine("spawnAllPieces");
        }
        errorText.SetActive(true);
    }

    IEnumerator PostMove()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", currentGame.gameId);
        form.AddField("history", currentGame.gameHistory);
        WWW www = new WWW(DBManager.BASEURL + "savegamestate.php", form);
        yield return www;
        //Debug.Log(www.text);
        if (www.text != "")
        {
            if (www.text[0] == '0')
            {
                //Debug.Log("saved new sate");
            }
        }
        else
        {
            Debug.Log(www.text);
        }
    }

}
