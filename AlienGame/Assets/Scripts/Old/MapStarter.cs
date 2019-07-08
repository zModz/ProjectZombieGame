using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapStarter : MonoBehaviour {

	public void OnMouseClick()
    {
        SceneManager.LoadScene ("Station", LoadSceneMode.Single);

    }
}
