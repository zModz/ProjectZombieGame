using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [Header("Loading")]
    public int sceneIndex;
    public Text LoadProgress;
    public PlayfabManager mana;

    // Start is called before the first frame update
    void Update()
    {
        if(mana.ready == true)
            StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {

            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            LoadProgress.text = progress * 100 + "%";
            yield return null;
        }
    }
}
