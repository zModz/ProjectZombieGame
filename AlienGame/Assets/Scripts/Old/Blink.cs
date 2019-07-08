using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {

	public float waitingTime = 0.05f;
	public Light light;

	IEnumerator Start ()
	{
		while (true)
		{
			light.enabled = !(light.enabled); //toggle on/off the enabled property
			yield return new WaitForSeconds(waitingTime);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
