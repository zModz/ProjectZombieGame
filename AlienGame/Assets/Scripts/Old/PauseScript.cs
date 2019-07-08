using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseScript : MonoBehaviour {

	public static bool GamePaused = false;
	public GameObject Player;
    public GameObject weaponManager;
    public WeaponManager _weaponManager;
    public GameObject Pause_Canvas;
	
	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if (GamePaused) 
			{
				resume ();
			} 
			else 
			{
				pause ();
			}
		}
	}

	void resume()
	{
		Pause_Canvas.SetActive (false);
		Time.timeScale = 1f;
		GamePaused = false;
		Player.GetComponent<FirstPersonController>().enabled = true;
        weaponManager.GetComponent<WeaponManager>().enabled = true;
        _weaponManager.weapons[_weaponManager.currentWeapon].gameObject.GetComponent<WeaponScript>().enabled = true;
    }


	void pause()
	{
		Pause_Canvas.SetActive (true);
		Time.timeScale = 0f;
		GamePaused = true;
		Player.GetComponent<FirstPersonController>().enabled = false;
        weaponManager.GetComponent<WeaponManager>().enabled = false;
        _weaponManager.weapons[_weaponManager.currentWeapon].gameObject.GetComponent<WeaponScript>().enabled = false;
    }


}
