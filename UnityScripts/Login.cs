using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField passwordField;

    public Button createUserButton;
    public Button loginUserButton;

    public GameController gameController;

    public TMP_Text errorText;

    Animator animator;

    private bool login = false;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void ToggleState()
    {
        login = !login;
        animator.SetBool("login", login);
    }

    public void SendRequest()
    {
        if (nameField.text.Length < 8 || passwordField.text.Length < 8)
        {
            errorText.gameObject.SetActive(false);
            errorText.text = "Username and/or password to short";
            errorText.gameObject.SetActive(true);
            return;
        }

        if (login)
        {
            StartCoroutine(LoginPlayer());
        }
        else
        {
            StartCoroutine(Register());
        }
    }

    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        WWW www = new WWW(DBManager.BASEURL+"login.php", form);
        yield return www;
        //Debug.Log(www.text);
        if (www.text != "")
        {
            if (www.text[0] == '0')
            {
                DBManager.username = nameField.text;
                nameField.text = "";
                passwordField.text = "";
                gameController.LoadPlayerData();
            }
            else
            {
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

    IEnumerator Register()
    {
        Debug.Log("REGISTER");
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        WWW www = new WWW(DBManager.BASEURL + "register.php", form);
        yield return www;
        if (www.text != "")
        {
            if (www.text == "0")
            {
                DBManager.username = nameField.text;
                nameField.text = "";
                passwordField.text = "";
                gameController.LoadPlayerData();
            }
            else
            {
                errorText.gameObject.SetActive(false);
                errorText.text = "User Creation Failed" + www.text;
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
