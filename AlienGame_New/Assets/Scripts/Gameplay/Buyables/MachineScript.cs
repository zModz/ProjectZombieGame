using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    [Header("Neccessary Variables")]
    public PerksManager Perk;
    public Player_Script pointManager;
    public SpriteRenderer perkIcon;
    public Transform eyesPoint;
    public int perkIndex;

    void Start()
    {
        pointManager = pointManager.GetComponent<Player_Script>();
        perkIcon = perkIcon.GetComponent<SpriteRenderer>();
        Perk = Perk.GetComponent<PerksManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var perkIndex in Perk.PerkList)
        {
            if (Perk.PerkList[this.perkIndex] == perkIndex)
            {
                perkIcon.sprite = Perk.PerkList[this.perkIndex].perkIcon;
            }
        }

        if (Physics.Raycast(eyesPoint.position, eyesPoint.transform.forward, out RaycastHit hit, 10))
        {
            Debug.Log(hit.transform + "/" + hit.transform.tag);
            if (hit.transform == gameObject.transform && hit.transform.tag == "PerkMachine")
            {
                BuyPerk(perkIndex);
            }

            /*if (hit.transform.tag != "PerkMachine" && hit.transform != gameObject.transform)
            {
                Debug.Log("BE GONE THOT");
                pointManager.UI_Alert.text = "";
            }*/
        }

    }
    
    void BuyPerk(int PerkNum)
    {
        if(Perk.PerkList[PerkNum].isActive != true && Perk.PerkList[PerkNum].isBuyable != false)
        {
            pointManager.UI_Alert.text = "Press F to buy " + Perk.PerkList[PerkNum].Name + " for " + Perk.PerkList[PerkNum].pointsToBuy;
            if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= Perk.PerkList[PerkNum].pointsToBuy)
            {
                pointManager.Points -= Perk.PerkList[PerkNum].pointsToBuy;
                Perk.PerkList[PerkNum].isActive = true;
                //Perk.PerkList[PerkNum].perkIcon_UI.sprite = Perk.PerkList[PerkNum].perkIcon;
            }
        }
        else { pointManager.UI_Alert.text = ""; }

        if (Perk.PerkList[PerkNum].isBuyable == false)
        {
            pointManager.UI_Alert.text = "You cannot buy this item at this time.";
        }
    }
}
