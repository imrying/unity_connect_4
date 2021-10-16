using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text username;

    public GameObject scrollViewContent;
    public GameObject gameViewPrefab;

    public GameObject loginPanel;
    public GameObject logoutButton;
    public GameObject createButton;
    public GameObject p1text;
    public GameObject p2text;
    public GameObject idtext;
    public GameObject waitingobj;
    public BoardController boardController;
    public GameObject wintext;

    Dictionary<int, GameObject> games = new Dictionary<int, GameObject>();

    float lastTime;

    void Update()
    {
        if (Time.time > lastTime && DBManager.username != null)
        {

            StartCoroutine(CheckForUpdates());
            lastTime = Time.time + 1.5f;
        }
    }

    public void LoadPlayerData()
    {
        username.text = DBManager.username;
        loginPanel.SetActive(false);
        logoutButton.SetActive(true);
        createButton.SetActive(true);
        p1text.SetActive(true);
        p2text.SetActive(true);
        idtext.SetActive(true);
        waitingobj.SetActive(true);
    }




    public void LogOutPlayer()
    {
        DBManager.LogOut();
        createButton.SetActive(false);
        loginPanel.SetActive(true);
        logoutButton.SetActive(false);
        p1text.SetActive(false);
        p2text.SetActive(false);
        idtext.SetActive(false);
        waitingobj.SetActive(false);
        DeleteUIForPlayer();
        username.text = "";
        wintext.SetActive(false);
    }

    private void SetupUIForGame(int id, string p1, string p2, string history)
    {
        GameObject UIButton = Instantiate(gameViewPrefab, scrollViewContent.transform);
        GameData gd = UIButton.GetComponent<GameData>();
        gd.gameId = id;
        gd.p1d = p1;
        gd.p2d = p2;
        gd.boardController = boardController;
        gd.gameHistory = history;
        games.Add(id, UIButton);
        gd.UpdateObjects();

    }

    private void DeleteUIForPlayer()
    {
        foreach(KeyValuePair<int,GameObject> entry in games)
        {
            Destroy(entry.Value);
        }
        games = new Dictionary<int, GameObject>();
    }


    IEnumerator GetGamesForPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("p1", DBManager.username);
        WWW www = new WWW(DBManager.BASEURL + "fetchallgames.php", form);
        yield return www;
        if (www.text != "")
        {
            //Debug.Log("In here");
            //Debug.Log(www.text);
            List<SingleGame> allGames = JsonConvert.DeserializeObject<List<SingleGame>>(www.text);
            allGames.Reverse();
            foreach (SingleGame game in allGames)
            {
                SetupUIForGame(int.Parse(game.id), game.p1, game.p2, game.history);
            }
            //Debug.Log("WOWWOoOOOO" + allGames[0].id);
        }
        else
        {
            //Debug.Log("Server Appears to be offline");
        }
    }

    IEnumerator CheckForUpdates()
    {
        WWWForm form = new WWWForm();
        form.AddField("p1", DBManager.username);
        WWW www = new WWW(DBManager.BASEURL + "fetchallgames.php", form);
        yield return www;
        if (www.text != "")
        {
            //Debug.Log("In here");
            //Debug.Log(www.text);
            List<SingleGame> allGames = JsonConvert.DeserializeObject<List<SingleGame>>(www.text);
            if (games.Count < allGames.Count)
            {
                DeleteUIForPlayer();
                StartCoroutine(GetGamesForPlayer());
                //Debug.Log("STOP FFS");
            }
            else
            {
                foreach (SingleGame game in allGames)
                {
                    //Debug.Log(game.history);
                    if (games[int.Parse(game.id)].GetComponent<GameData>().gameHistory != null && game.history != null)
                    {
                        if (games[int.Parse(game.id)].GetComponent<GameData>().gameHistory != game.history)
                        {
                            games[int.Parse(game.id)].GetComponent<GameData>().gameHistory = game.history;
                            games[int.Parse(game.id)].GetComponent<GameData>().UpdateObjects();
                            games[int.Parse(game.id)].GetComponent<GameData>().boardController.SelectMe(games[int.Parse(game.id)].GetComponent<GameData>(), true);
                        }
                    }
                }
            }
        }
        else
        {
            //Debug.Log("Server Appears to be offline");
        }
    }

    public class SingleGame{
        public string p1 { get; set; }
        public string p2 { get; set; }
        public string id { get; set; }
        public string history { get; set; }
    }
}
