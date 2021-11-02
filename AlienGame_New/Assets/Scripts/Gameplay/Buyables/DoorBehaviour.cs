using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorBehaviour : MonoBehaviour
{
    public bool isOpen;
    public int pointsToBuy;
    [SerializeField]
    int timer;

    [Header("Game Objects")]
    public SphereCollider area;
    public GameObject explosive;
    public TextMeshPro explo_timer;
    public GameObject door;
    public Player_Script player;

    private void Start()
    {
        explosive.SetActive(false);
        area = area.GetComponent<SphereCollider>();
        player = player.GetComponent<Player_Script>();
        Debug.Log(gameObject.tag);
    }

    IEnumerator BuyDoor()
    {
        player.Points -= pointsToBuy;
        explosive.SetActive(true);
        int temp = timer;
        temp = temp - 1;
        explo_timer.text = "00:0" + temp;
        yield return new WaitForSeconds(timer);
        isOpen = true;
        explosive.SetActive(false);
        door.SetActive(false);
        area.enabled = false;
        player.UI_Alert.text = "";
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject.tag == "Door")
        {
            Debug.Log("HERE!");
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("HERE AGAIN!");
                if (!isOpen)
                {
                    player.UI_Alert.text = "Press F to open (Cost: " + pointsToBuy + ")";
                    if (Input.GetKeyDown(KeyCode.F) && player.Points >= pointsToBuy)
                    {
                        StartCoroutine(BuyDoor());
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
                Debug.Log("out");
                player.UI_Alert.text = "";
            }
        }
    }
}
