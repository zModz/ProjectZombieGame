using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameJolt;
using GameJolt.API;

public class WeaponScript : MonoBehaviour
{
    GameObject player;
    PlayerScript playerpoints;
    int mouseclicks;
    public enum FireModes { Auto, Semi, Burst };

    //Weapon Atributes
    [Header("Weapon Atributes")]
    public String WeaponName;
    public Text WeaponNameText;
    public Text BulletsText;
    new AudioSource audio;
    public AudioClip firesound;
    public AudioClip reloadsound;
    string Reload = "LOW AMMO";
	string NoAmmo = "NO AMMO";

	//Weapon Stats
	[Header("Weapon Stats")]
    public FireModes fireModes;

    [HideInInspector]
    public int currentBullets;
    public int bulletPerMag = 30;
    public int bulletsLeft;
    public int maxAmmo = 120;
    public float ReloadTime;

    public int damage;
	public float ADSSpeed;
    public float range = 100f;
    float fireTimer;
    public float fireRate = 0.1f;
    float InicialSpread;
    public float normalSpread = 0.04f;
    
	//Other Atributes
	[Header("Other Atributes")]
    public Vector3 adsPos;
    public GameObject crosshair;
    public GameObject bulletHole;
    public GameObject bloodparticle;
    public GameObject muzzleflash;
    public Transform shootPoint;

    Vector3 originalPos;
    bool IsReloading = false;
    Animation anim;
    string fireMode = "";


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
		audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerpoints = player.GetComponent<PlayerScript>();
        originalPos = transform.localPosition;

        anim.Play ("draw");

        currentBullets = bulletPerMag;
        InicialSpread = normalSpread;
    }

    // Update is called once per frame
    void Update()
    {
        SetBulletsText();
        SetWeaponName();

        //Fire Rate
        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        //Fire Weapon
        if (fireModes == FireModes.Auto)
        {
            fireMode = "FULL";
            if (Input.GetButton("Fire1"))
            {
                if (IsReloading == false && currentBullets > 0)
                {
                    Fire();
                }
                else if (currentBullets <= 0 && bulletsLeft <= 0)
                {
                    IsReloading = false;
                }
                else
                {
                    StartCoroutine("ReloadFix");
                }
            }
        }
        else if (fireModes == FireModes.Semi)
        {
            fireMode = "SEMI";
            if (Input.GetButtonDown("Fire1"))
            {
                if (IsReloading == false && currentBullets > 0)
                {
                    Fire();
                }
                else if (currentBullets <= 0 && bulletsLeft <= 0)
                {
                    IsReloading = false;
                }
                else if (currentBullets == 0)
                {
                    StartCoroutine("ReloadFix");
                }
            }
        }
        else if (fireModes == FireModes.Burst)
        {
            // Burst Fire
        }
        
        //Reload (Press)
        if (Input.GetButton("Reload"))
        {
            if (currentBullets < bulletPerMag && bulletsLeft > 0)
            {
                StartCoroutine("ReloadFix");
            }

        }

        //Reload (Auto)
        if( currentBullets == 0 && currentBullets < bulletPerMag && bulletsLeft > 0)
        {
            StartCoroutine("ReloadFix");
        }

        //Aiming
		if (IsReloading == false) 
		{
			ADS ();
		}
		else 
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, originalPos, Time.deltaTime * ADSSpeed);
			crosshair.SetActive (true);
		}
    }
    
	void Sprint() 
	{
		if (Input.GetKey ("w") && Input.GetButtonDown ("Sprint")) {
			anim.CrossFade ("sprint");
		} 
		else 
		{
			anim.Stop ("sprint");
		}
	}

    IEnumerator ReloadFix()
    {
        IsReloading = true;
		transform.localPosition = Vector3.Lerp (transform.localPosition, originalPos, Time.deltaTime);
		audio.clip = reloadsound;
		audio.Play ();
		anim.Stop("fire");
        anim.Play("reload");
		yield return new WaitForSeconds(ReloadTime);
        ReloadFun();
        IsReloading = false;
    }

    void ADS ()
	{
		if (Input.GetButton ("Fire2")) {
            normalSpread = 0;
            transform.localPosition = Vector3.Lerp (transform.localPosition, adsPos, Time.deltaTime * ADSSpeed);
			crosshair.SetActive (false);
		}
        else
        {
            normalSpread = InicialSpread;
            transform.localPosition = Vector3.Lerp (transform.localPosition, originalPos, Time.deltaTime * ADSSpeed);
			crosshair.SetActive (true);
		}
	}

    void ReloadFun()
    {
        if (IsReloading == true)
        {
            if (bulletsLeft <= 1) return;

            int bulletToLoad = bulletPerMag - currentBullets;
            int bulletsToDeduct = (bulletsLeft >= bulletToLoad) ? bulletToLoad : bulletsLeft;

            bulletsLeft -= bulletsToDeduct;
            currentBullets += bulletsToDeduct;
        }
    }
    
    void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0) return;
        Debug.Log("Fired");

        RaycastHit hit;
        Vector3 direction = shootPoint.transform.forward;
        direction.x += UnityEngine.Random.Range(-normalSpread, normalSpread);
        direction.y += UnityEngine.Random.Range(-normalSpread, normalSpread);
        direction.z += UnityEngine.Random.Range(-normalSpread, normalSpread);

        if (Physics.Raycast(shootPoint.position, direction, out hit, range))
        {
            Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			Debug.Log ("Hit " + hit.collider.name + " (" + hit.collider.tag + ")");

			if (hit.transform.tag == "Enemy") {
                Instantiate(bloodparticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                ZombieScript enemyHealth = hit.collider.GetComponent <ZombieScript> ();
				Debug.Log ("Hit " + hit.transform.tag + " - TOOK DAMAGE: " + damage);
                playerpoints.Points += 10;
				enemyHealth.TakeDamage (damage);
			}
        }
			
        anim.Stop();
        anim.CrossFade("fire");
		audio.clip = firesound;
		audio.Play ();

        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        currentBullets--;
        fireTimer = 0.0f;
    }

    void SetBulletsText()
    {
        BulletsText.text = currentBullets.ToString() + "/" + bulletsLeft.ToString();
    }

    void SetWeaponName()
    {
        WeaponNameText.text = WeaponName.ToString() + " - " + fireMode.ToString();
    }

}