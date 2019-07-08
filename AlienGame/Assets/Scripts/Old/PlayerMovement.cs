using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public int playerSpeed;
    public Camera camera;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
        
        transform.Translate(x, 0, z);

        Camera camera = GetComponent<Camera>();
        transform.LookAt(camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane)), Vector3.up);
    }
}
