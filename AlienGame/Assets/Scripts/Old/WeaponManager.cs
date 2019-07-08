using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

	public GameObject[] weapons;

	public int currentWeapon;

	private int nrWeapons;


	// Use this for initialization
	void Start () {
		nrWeapons = weapons.Length;
		SwitchWeapon (currentWeapon);
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 1; i <= nrWeapons; i++) { 
			if (Input.GetKeyDown ("" + i)) {
				currentWeapon = i - 1;

				SwitchWeapon (currentWeapon);

			}
		}
	}

	public void SwitchWeapon(int index) {

		for (int i=0; i < nrWeapons; i++)    {
			if (i == index) {
				weapons[i].gameObject.SetActive(true);
			} else { 
				weapons[i].gameObject.SetActive(false);
			}
		}
	}
}
