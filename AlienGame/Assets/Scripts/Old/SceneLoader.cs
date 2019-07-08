using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public Slider slider;
    public int scene;

	// Use this for initialization
	void Awake ()
    {
        LoadScene(scene);
	}

    void LoadScene(int SceneIndex)
    {
        StartCoroutine(Load(SceneIndex));
    }
	
    IEnumerator Load(int SceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneIndex);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            slider.value = progress;

            yield return null;
        }
    }
	
}
