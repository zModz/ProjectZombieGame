using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitmarkerScript : MonoBehaviour {
	public Image hitmarker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getHitmarker() 
	{
		StopCoroutine ("GotHit");
		hitmarker.enabled = true;
		StartCoroutine ("GotHit");
	}

	IEnumerator GotHit() 
	{
		yield return new WaitForSeconds (0.1f);
		hitmarker.enabled = false;
	}
}
