using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class maps
{
    public string MapName;
    public string MapDesc;
    public Scene MapScene;
}


public class MapManager : MonoBehaviour
{
    private static MapManager _instance = null;
    public static MapManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public maps[] MapsList = new maps[5];
}
