using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMsgBehaviour : MonoBehaviour
{
    public GameObject ErrorMsgBox;
    public Text Upper_Text;
    public Text Down_Text;
    public Button button;
    //bool isButton;

    private void Start()
    {
    }

    public void ErrorMsg(string upperText, string downText, bool isDisplayed)
    {
        ErrorMsgBox.SetActive(true);
        Time.timeScale = 0.0f;

        Upper_Text.text = upperText;
        Down_Text.text = downText;
        button.gameObject.SetActive(isDisplayed);
    }
}
