using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool isOpen;
    public int pointsToBuy;
    Animator anim;
    SphereCollider sphere;
    public Player_Script player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sphere = GetComponent<SphereCollider>();
        player = player.GetComponent<Player_Script>();
    }

    public void BuyDoor()
    {
        player.Points -= pointsToBuy;
        //Debug.Log(player.Points + " - " + pointsToBuy);
        isOpen = true;
        anim.SetBool("Trigger_Open", true);
        sphere.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject.tag == "Door")
        {
            if (other.gameObject.tag == "Player")
            {
                //Debug.Log("D_in");
                if (!isOpen)
                {
                    anim.SetBool("Trigger_Open", false);
                    player.UI_Alert.text = "Press F to open (Cost: " + pointsToBuy + ")";
                    if (Input.GetKeyDown(KeyCode.F) && player.Points >= pointsToBuy)
                    {
                        BuyDoor();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "Door")
        {
            if (other.gameObject.tag == "Player")
            {
                //Debug.Log("out");
                player.UI_Alert.text = "";
            }
        }
    }
}
