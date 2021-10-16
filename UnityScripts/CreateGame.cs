using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateGame : MonoBehaviour
{
    public TMP_Text errorText;
    public TMP_InputField opponentName;

    public void StartGameWithUser()
    {
        if (opponentName.text.Length < 8)
        {
            errorText.gameObject.SetActive(false);
            errorText.text = "Name too short to exist";
            errorText.gameObject.SetActive(true);
            return;
        }
        else if(DBManager.username == opponentName.text)
        {
            errorText.gameObject.SetActive(false);
            errorText.text = "You cannot play with yourself silly...";
            errorText.gameObject.SetActive(true);
            return;
        }
        StartCoroutine(StartGame());

    }

    IEnumerator StartGame()
    {
        WWWForm form = new WWWForm();
        if (Random.Range(0, 1) < 0.5f)
        {
            form.AddField("p1", DBManager.username.ToString());
            form.AddField("p2", opponentName.text);
        }
        else
        {
            form.AddField("p1", opponentName.text);
            form.AddField("p2", DBManager.username.ToString());
        }



        WWW www = new WWW(DBManager.BASEURL + "creategame.php", form);
        yield return www;
        Debug.Log(www.text);
        if (www.text != "")
        {
            if (www.text[0] == '0')
            {
                Debug.Log("Creatino succesful");
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(www.text);
                errorText.gameObject.SetActive(false);
                errorText.text = "User not in database";
                errorText.gameObject.SetActive(true);

            }
        }
        else
        {
            errorText.gameObject.SetActive(false);
            errorText.text = "Server appears to be offline";
            errorText.gameObject.SetActive(true);
        }
    }
}
