using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreboardBehaviour : MonoBehaviour
{
    public Sprite p_lvl;
    public string p_name = "";
    public float p_score = 0;
    public float p_kills = 0;
    public float p_headshots = 0;
    public float p_revives = 0;
    public float p_downs = 0;

    [Header("Properties")]
    public Image lvlImage;
    public Text playerName;
    public Text score;
    public Text kills;
    public Text headshots;
    public Text revives;
    public Text downs;
    public Text mapName;
    public GameManager Manager;
    public Player_Script player;
    public int scene;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(scene + " " + SceneManager.GetActiveScene().name);

        foreach (var MapIndex in Manager.MapsList)
        {
            for(int i = 0; i < Manager.MapsList.Length; i++)
            {
                if (Manager.MapsList[i].MapScene.handle == scene)
                {
                    mapName.text = Manager.MapsList[i].MapName;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        lvlImage.sprite = p_lvl;
        playerName.text = p_name;
        p_score = player.Points;
        score.text = p_score.ToString();
        kills.text = p_kills.ToString();
        headshots.text = p_headshots.ToString();
        revives.text = p_revives.ToString();
        downs.text = p_downs.ToString();
    }
}
