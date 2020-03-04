using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallBuyScript : MonoBehaviour
{
    [Header("Neccessary Variables")]
    public bool isUsed;
    public WeaponManager Wpn;
    public Player_Script pointManager;
    public SpriteRenderer WpnIcon;
    public Transform eyesPoint;
    public Text text;
    public int WpnIndex;

    void Start()
    {
        pointManager = pointManager.GetComponent<Player_Script>();
        WpnIcon = WpnIcon.GetComponent<SpriteRenderer>();
        Wpn = Wpn.GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var wpnIndex in Wpn.WeaponList)
        {
            if (Wpn.WeaponList[this.WpnIndex] == wpnIndex)
            {
                WpnIcon.sprite = Wpn.WeaponList[this.WpnIndex].WeaponIcon_buy;
            }
        }

        if (Physics.Raycast(eyesPoint.position, eyesPoint.transform.forward, out RaycastHit hit))
        {
            //Debug.Log(hit.transform + "/" + hit.transform.tag);
            if (hit.transform == gameObject.transform && hit.transform.tag == "WallBuy" && hit.distance < 2)
            {
                Buy(WpnIndex);
            }
            else
            {
                text.text = "";
            }
        }
    }
    
    void Buy(int WpnNum)
    {
        if(isUsed != true && Wpn.WeaponList[WpnNum].isBuyable != false)
        {
            text.text = "Press F to buy " + Wpn.WeaponList[WpnNum].name + " for " + Wpn.WeaponList[WpnNum].pointsToBuy;
            if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= Wpn.WeaponList[WpnNum].pointsToBuy)
            {
                isUsed = true;
                pointManager.Points -= Wpn.WeaponList[WpnNum].pointsToBuy;
                StartCoroutine(Buy());
            }
        }
        else if (isUsed)
        {
            text.text = "Press F to buy " + Wpn.WeaponList[WpnNum].name + " ammo for " + Wpn.WeaponList[WpnNum].pointsToBuyAmmo + " or buy again for " + Wpn.WeaponList[WpnNum].pointsToBuy;
        }

        if (Wpn.WeaponList[WpnNum].isBuyable == false)
        {
            text.text = "You cannot buy this item at this time.";
        }
    }

    IEnumerator Buy()
    {
        pointManager.nrWpn = 2;
        yield return new WaitForSeconds(0.1f);
        pointManager.activeWeapons[1].WpnIndex = WpnIndex;
        pointManager.SwitchWeapon(1);
    }
}
