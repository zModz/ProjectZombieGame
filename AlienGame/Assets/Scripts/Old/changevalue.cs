using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changevalue : MonoBehaviour {

    public Slider FOV;
    float aa;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void OnFOVChange()
    {
        Text text = GetComponent<Text>();
        aa = FOV.value;
        text.text = aa.ToString();
    }
}
