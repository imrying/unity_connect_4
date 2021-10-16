using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateLink : MonoBehaviour
{
    public TMP_InputField inpt;

    public void UpdateLinkF()
    {
        DBManager.BASEURL = inpt.text;
    }
}
