using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapLoading : MonoBehaviour
{
    public GameObject MapManagerGO;
    GameManager MapManager;
    
    public int MapIndex;

    public Text LoadProgress;

    // Start is called before the first frame update
    void Start()
    {
        MapManager = MapManagerGO.GetComponent<GameManager>();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        foreach (var MapIndex in MapManager.MapsList)
        {
            if (MapManager.MapsList[this.MapIndex] == MapIndex)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MapManager.MapsList[this.MapIndex].MapScene.handle);

                // Wait until the asynchronous scene fully loads
                while (!asyncLoad.isDone)
                {

                    float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

                    LoadProgress.text = progress * 100 + "%";
                    yield return null;
                }
            }
        }
    }
}
