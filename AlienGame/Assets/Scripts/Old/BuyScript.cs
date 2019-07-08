using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyScript : MonoBehaviour {
    
    public int costToBuy;
    public int costToRefill;
    public Transform eyesPoint;
    public PlayerScript pointManager;
    public GameObject weaponToBuy;
    public WeaponManager weaponManager;
    public WeaponScript weaponScript;

	// Use this for initialization
	void Start ()
    {
        pointManager = pointManager.GetComponent<PlayerScript>();
        weaponManager = weaponManager.GetComponent<WeaponManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {    
        weaponScript = weaponManager.weapons[weaponManager.currentWeapon].gameObject.GetComponent<WeaponScript>();

        RaycastHit hit;
        if (Physics.Raycast(eyesPoint.position, eyesPoint.transform.forward, out hit, 10))
        {
            if (hit.transform == gameObject.transform)
            {
                BuyWeapon();
                BuyAmmo();
            }
        }
    }

    void BuyWeapon()
    {
        //Buy Weapon
        if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= costToBuy && weaponToBuy != weaponManager.weapons[weaponManager.currentWeapon])
        {
            pointManager.Points -= costToBuy;
            weaponManager.weapons[1] = weaponToBuy;
            weaponManager.SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= costToBuy && weaponToBuy != weaponManager.weapons[weaponManager.currentWeapon] && weaponManager.weapons[1] == weaponToBuy)
        {
            pointManager.Points -= costToBuy;
            weaponManager.weapons[weaponManager.currentWeapon] = weaponToBuy;
        }
    }

    void BuyAmmo()
    {
        //Buy Ammo
        if (Input.GetKeyDown(KeyCode.F) && pointManager.Points >= costToRefill && weaponToBuy == weaponManager.weapons[weaponManager.currentWeapon])
        {
            pointManager.Points -= costToRefill;
            weaponScript.currentBullets = weaponScript.bulletPerMag;
            weaponScript.bulletsLeft = weaponScript.maxAmmo;
        }
    }
}
