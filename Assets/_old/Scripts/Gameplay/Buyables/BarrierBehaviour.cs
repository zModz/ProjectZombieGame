using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    public GameObject[] barriers = new GameObject[3];
    public bool isDestroyed;
    public Player_Script player;
    public int pointToGive;


    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
        player = player.GetComponent<Player_Script>();
    }

    private void Update()
    {
        for (int i = 0; i < barriers.Length; i++){
            if (barriers[i].active == false)
            {
                isDestroyed = true;
            }
        }
    }

    void GivePoints(int num)
    {
        barriers[num].SetActive(true);
        player.Points += pointToGive;
        Debug.Log(player.Points += pointToGive);
    }

    IEnumerator Barrier()
    {
        yield return new WaitForSeconds(1f);
        GivePoints(0);
        yield return new WaitForSeconds(1f);
        GivePoints(1);
        yield return new WaitForSeconds(1f);
        GivePoints(2);
        player.UI_Alert.text = "";
        isDestroyed = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject.tag == "Barrier")
        {
            if (other.gameObject.tag == "Player")
            {
                //Debug.Log("B_in");
                if (isDestroyed)
                {
                    //anim.SetBool("Trigger_Open", false);
                    player.UI_Alert.text = "Press F to rebuild barrier";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        StartCoroutine(Barrier());
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "Barrier")
        {
            if (other.gameObject.tag == "Player")
            {
                //Debug.Log("out");
                player.UI_Alert.text = "";
            }
        }
    }
}
