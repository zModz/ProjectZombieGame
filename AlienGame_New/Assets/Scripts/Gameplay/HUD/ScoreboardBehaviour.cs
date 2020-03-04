using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreboardBehaviour : MonoBehaviour
{
    float p_score = 0;
    float p_kills = 0;
    float p_headshots = 0;
    float p_revives = 0;
    float p_downs = 0;
    public Text score;
    public Text kills;
    public Text headshots;
    public Text revives;
    public Text downs;
    public Text mapName;
    public MapManager mapManager;
    public Player_Script player;
    public int scene;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(scene + " " + SceneManager.GetActiveScene().name);


        p_score = player.Points;
        score.text = p_score.ToString();
        kills.text = p_kills.ToString();
        headshots.text = p_headshots.ToString();
        revives.text = p_revives.ToString();
        downs.text = p_downs.ToString();



        foreach (var MapIndex in mapManager.MapsList)
        {
            for(int i = 0; i < mapManager.MapsList.Length; i++)
            {
                if (mapManager.MapsList[i].MapScene.handle == scene)
                {
                    Debug.Log("Did it got here");
                    mapName.text = mapManager.MapsList[i].MapName;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        p_score = player.Points;
        score.text = p_score.ToString();
        kills.text = p_kills.ToString();
        headshots.text = p_headshots.ToString();
        revives.text = p_revives.ToString();
        downs.text = p_downs.ToString();
    }
}
