using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MapBase : MonoBehaviour
{
    public GameManager Maps;
    public int MapIndex;
    public Text MapName_Text;
    public Text MapLoadProgress;

    [Header("Loading")]
    public int LoadingScene;
    public GameObject mapNameGO;
    public GameObject mapDescGO;
    public GameObject mapImgGO;
    public GameObject MapLoadGO;
    Text mapName;
    Text mapDesc;
    Image mapImg;
    MapLoading MapLoad;

    public DiscordController disc;

    // Start is called before the first frame update
    void Start()
    {
        //Maps = Maps.GetComponent<GameManager>();

        mapName = mapNameGO.GetComponent<Text>();
        mapDesc = mapDescGO.GetComponent<Text>();
        mapImg = mapImgGO.GetComponent<Image>();
        MapLoad = MapLoadGO.GetComponent<MapLoading>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var MapIndex in Maps.MapsList)
        {
            if(Maps.MapsList[this.MapIndex] == MapIndex)
            {
                MapName_Text.text = Maps.MapsList[this.MapIndex].MapName.ToString().ToUpper();
            }
        }
    }

    public void Click()
    {
        Debug.Log("Map Name: " + Maps.MapsList[this.MapIndex].MapName.ToString());
        StartCoroutine(LoadScene());
        MapLoad.MapIndex = MapIndex;

        mapName.text = Maps.MapsList[this.MapIndex].MapName.ToString().ToUpper();
        mapDesc.text = Maps.MapsList[this.MapIndex].MapDesc.ToString().ToUpper();
        mapImg.sprite = Maps.MapsList[this.MapIndex].MapImage;

        disc.UpdateActivity("Playing on " + mapName.text, "Surviving Solo");
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadingScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            MapLoadProgress.text = asyncLoad.progress.ToString();
            yield return null;
        }
    }
}
