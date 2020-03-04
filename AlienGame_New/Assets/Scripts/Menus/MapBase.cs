using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MapBase : MonoBehaviour
{
    public MapManager Maps;
    public int MapIndex;
    public Text MapName_Text;
    public Text MapLoadProgress;

    [Header("Loading")]
    public int LoadingScene;
    public GameObject mapNameGO;
    public GameObject mapDescGO;
    public GameObject MapLoadGO;
    Text mapName;
    Text mapDesc;
    MapLoading MapLoad;

    // Start is called before the first frame update
    void Start()
    {
        Maps = Maps.GetComponent<MapManager>();

        mapName = mapNameGO.GetComponent<Text>();
        mapDesc = mapDescGO.GetComponent<Text>();
        MapLoad = MapLoadGO.GetComponent<MapLoading>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var MapIndex in Maps.MapsList)
        {
            if(Maps.MapsList[this.MapIndex] == MapIndex)
            {
                Debug.Log("Map Name: " + Maps.MapsList[this.MapIndex].MapName.ToString());
                MapName_Text.text = Maps.MapsList[this.MapIndex].MapName.ToString();

                mapName.text = Maps.MapsList[this.MapIndex].MapName.ToString();
                mapDesc.text = Maps.MapsList[this.MapIndex].MapDesc.ToString();
            }
        }
    }

    public void Click()
    {
        StartCoroutine(LoadScene());
        MapLoad.MapIndex = MapIndex;
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
