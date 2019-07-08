using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyDoorScript : MonoBehaviour {
    
    public int costToBuy;
    public GameObject door;
    public Transform eyesPoint;
    public PlayerScript pointManager;
    public Text BuyInfo;

    // Use this for initialization
    void Start ()
    {
        pointManager = pointManager.GetComponent<PlayerScript>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyesPoint.position, eyesPoint.transform.forward, out hit, 10))
        {
            if (hit.transform == gameObject.transform)
            {
                BuyDoor();
                BuyInfo.text = "Open door (" + costToBuy + ")";
            }
        }
    }

    void BuyDoor()
    {
        //Buy Door
        if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= costToBuy)
        {
            //Vector3 pos = new Vector3(door.transform.localPosition.x, door.transform.localPosition.y, -100f);
            //door.transform.localPosition = Vector3.Lerp(door.transform.localPosition, pos, Time.deltaTime * 5);
            door.SetActive(false);
            pointManager.Points -= costToBuy;
        }
    }
}
