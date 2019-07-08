using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    public float Amount;
    public float smoothAmount;
    public float maxAmount;

    private Vector3 InitialPos;

	// Use this for initialization
	void Start ()
    {
        InitialPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float moveX = -Input.GetAxis("Mouse X") * Amount;
        float moveY = -Input.GetAxis("Mouse Y") * Amount;
        moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
        moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + InitialPos, Time.deltaTime * smoothAmount);
    }
}
