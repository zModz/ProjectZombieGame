using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WallBuyScript : MonoBehaviour
{
    public Transform obj;
    [Header("Neccessary Variables")]
    public bool isUsed;
    //WeaponSettings Wpn;
    public WeaponManager Manager;
    public Player_Script pointManager;
    public WeaponSwitch weaponManager;
    public SpriteRenderer WpnIcon;
    public Transform eyesPoint;
    public Text text;
    public int WpnIndex;

    void Start()
    {
        pointManager = pointManager.GetComponent<Player_Script>();
        WpnIcon = WpnIcon.GetComponent<SpriteRenderer>();
        obj = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (WpnIndex == Manager.weaponList[WpnIndex].WeaponIndex)
        {
           var Wpn = Manager.weaponList[WpnIndex];
           WpnIcon.sprite = Wpn.WeaponIconBuy;


           //CHANGE THIS TO COLLIDERS INSTEAD OF RAYCAST
            if (Physics.Raycast(eyesPoint.position, eyesPoint.transform.forward, out RaycastHit hit))
            {
                //Debug.Log(hit.transform + "/" + hit.transform.tag);
                if (hit.transform == obj && obj.tag == "WallBuy" && hit.distance < 2)
                {
                    Debug.Log(transform.name + " / " + Manager.weaponList[WpnIndex].WeaponIndex + " / " + WpnIndex + " / " + Manager.weaponList[WpnIndex].WeaponName);
                    this.Buy(this.WpnIndex);
                }
                else
                {
                    text.text = "";
                }
            }
        }
    }
    
    void Buy(int index)
    {
        string _text = "Press F to buy " + "(" + Manager.weaponList[index].WeaponIndex + ")" + Manager.weaponList[index].WeaponName + " for " + Manager.weaponList[index].pointsToBuy;
        //Debug.Log("BuyFunc: " + index);
        if (isUsed != true && Manager.weaponList[index].isBuyable != false)
        {
            if(text.text == "")
            {
                text.text = _text;
            }
            Debug.Log(text.text);
            if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= Manager.weaponList[index].pointsToBuy)
            {
                isUsed = true;
                pointManager.Points -= Manager.weaponList[index].pointsToBuy;
                StartCoroutine(BuyBehave());
            }
        }
        else if (isUsed)
        {
            text.text = "Press F to buy " + Manager.weaponList[index].WeaponName + " ammo for " + Manager.weaponList[index].pointsToBuyAmmo + " or buy again for " + Manager.weaponList[index].pointsToBuy;
        }

        if (Manager.weaponList[index].isBuyable == false)
        {
            text.text = "You cannot buy this item at this time.";
        }
    }

    IEnumerator BuyBehave()
    {
        weaponManager.nrWpn = 2;
        yield return new WaitForSeconds(0.1f);
        weaponManager.activeWeapons[1].WpnIndex = WpnIndex;
        weaponManager.currentWpn = weaponManager.activeWeapons[1].WpnIndex;
        weaponManager.SwitchWeapon(weaponManager.currentWpn);
    }
}
