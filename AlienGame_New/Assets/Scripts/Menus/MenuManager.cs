using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameManager GM;
    //Game Info
    [Header("Game Info")]
    public Text GameVersion_Text;
    public enum state { Alpha, BETA, Final };
    public state State;
    public Button DevMap_btn;
    public Button OnlineBtn;

    [Header("Player Info")]
    public Text playerName;
    public Text temp_name;

    void Awake()
    {
        
        if (Application.isEditor || Debug.isDebugBuild)
        {
            DevMap_btn.gameObject.SetActive(true);
        }
        else
        {
            DevMap_btn.gameObject.SetActive(false);
        }

        GameVersion_Text.text = Application.productName + " " + State + ".V" + Application.version + " (" + Application.unityVersion + ")";

        //Debug.Log(SystemInfo.graphicsDeviceName);
    }

    private void Update()
    {
        temp_name.text = "Logged in as: " + GM.playerName.ToString();
        if (GM.isLoggedIn == true)
        {
            OnlineBtn.interactable = false;
        }
    }
}
