using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //Game Info
    [Header("Game Info")]
    public Text GameVersion_Text;
    public enum state { Alpha, BETA, Final };
    public state State;
    public Button DevMap_btn;

    void Awake()
    {
        /*
        if (Application.isEditor)
        {
            DevMap_btn.gameObject.SetActive(true);
        }
        else
        {
            DevMap_btn.gameObject.SetActive(false);
        }*/

        GameVersion_Text.text = "Version: " + State + "." + Application.version + " (" + Application.unityVersion + ")";


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
