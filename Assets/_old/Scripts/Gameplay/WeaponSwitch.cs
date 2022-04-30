using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct ActiveWeapon
{
    public int WpnIndex;
    public GameObject WpnGO;
    ///public GameObject ArmsGO;
}

public class WeaponSwitch : MonoBehaviour
{
    public GameManager GM;
    [Tooltip("Each weapon position is the same as the Weapon Settings array")]
    public GameObject[] weaponList;

    [Header("Weapon Manager")]
    public Transform WpnList;
    WeaponSettings Weapons;
    public int currentWpn;
    [Range(1, 3)]
    public int nrWpn = 1;
    int WpnIdx;

    public ActiveWeapon[] activeWeapons;

    void Awake()
    {
        activeWeapons = new ActiveWeapon[nrWpn];
        weaponList = new GameObject[WpnList.childCount];
        //Lists
        int w = 0;
        foreach (Transform clild in WpnList) //Fills list
        {
            weaponList[w] = clild.gameObject;
            w++;
        }
    }

    void Start()
    {
        //1st slot creation
        activeWeapons[0].WpnIndex = GM.startingWeapon;
        currentWpn = activeWeapons[0].WpnIndex;
        activeWeapons[0].WpnGO = weaponList[currentWpn].gameObject;
        SwitchWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {

        //Resize Weapon
        for (int x = 1; x < nrWpn; x++)
        {
            Array.Resize(ref activeWeapons, nrWpn);
        }

        //Switch
        for (int i = 0; i < activeWeapons.Length; i++)
        {
            if (activeWeapons.Length >= 2)
            {
                if (Input.GetKeyDown("" + i))
                {
                    Debug.Log(i);
                    switch (i)
                    {
                        case 1:
                            Debug.Log("Current Weapon: " + i);
                            currentWpn = activeWeapons[0].WpnIndex;
                            SwitchWeapon(0);
                            break;
                        case 2:
                            Debug.Log("Current Weapon: " + i);
                            currentWpn = activeWeapons[1].WpnIndex;
                            SwitchWeapon(1);
                            break;
                        case 3:
                            Debug.Log("Current Weapon: " + i);
                            currentWpn = activeWeapons[2].WpnIndex;
                            SwitchWeapon(2);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void SwitchWeapon(int weaponSlot)
    {
        activeWeapons[weaponSlot].WpnGO.SetActive(true);
    }
}
