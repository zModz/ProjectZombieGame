using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    [Header("Neccessary Variables")]
    public PlayerScript Perk;
    public PlayerScript pointManager;
    public SpriteRenderer perkIcon;
    public Transform eyesPoint;

    [Header("Perk Variables")]
    [Tooltip("Perk Index goes from 0-??")]
    public int perkIndex;
    public int costToBuy;

    void Start()
    {
        pointManager = pointManager.GetComponent<PlayerScript>();
        perkIcon = perkIcon.GetComponent<SpriteRenderer>();
        Perk = Perk.GetComponent<PlayerScript>();
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
        
        RaycastHit hit;
        if (Physics.Raycast(eyesPoint.position, eyesPoint.transform.forward, out hit, 10))
        {
            if (hit.transform == gameObject.transform)
            {
                Debug.Log(hit.transform);
                BuyPerk(perkIndex);
            }
        }
    }

    void BuyPerk(int PerkNum)
    {
        if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= costToBuy)
        {
            Debug.Log(PerkNum);
            pointManager.Points -= costToBuy;
            Perk.PerkList[PerkNum].isActive = true;
            var temp = Perk.PerkList[PerkNum].perkIcon_UI.color;
            temp.a = 1f;
            Perk.PerkList[PerkNum].perkIcon_UI.color = temp;
        }
    }
}
