using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponBase : MonoBehaviour
{
    public enum WeaponType { Assault, SMG, LMG, Sniper, Shotgun, Pistol, Melee, Launcher }
    public WeaponType weaponType;
    public enum WeaponFireMode { Auto, Semi, Burst, Melee }
    public WeaponFireMode fireMode;

    //Weapon Atributes
    [Header("Weapon Atributes")]
    public string WeaponName;
    public Text WeaponNameText;
    public Text BulletsText;
    new AudioSource audio;
    public AudioClip firesound;
    public AudioClip reloadsound;
    public Transform shootPoint;
    [SerializeField]
    Vector3 originalPos;
    public Vector3 adsPos;
    [SerializeField]
    bool IsReloading = false;

    [HideInInspector]
    public int currentBullets;
    [Header("Weapon Stats")]
    public int bulletPerMag = 30;
    public int bulletsLeft;
    public int maxAmmo = 120;
    public float ReloadTime;
    public int damage;
    public float ADSSpeed;
    public float rangeMeters = 10;
    [SerializeField]
    float rangeUnit;
    float fireTimer;
    public float fireRate = 0.1f;
    [Header("Recoil")]
    public float normalSpread = 0.04f;
    float InicialSpread;

    [Header("Other Effects")]
    public GameObject crosshair;
    public GameObject bulletHole;
    public GameObject muzzleflash;


    // Start is called before the first frame update
    void Start()
    {
        currentBullets = bulletPerMag;
        bulletsLeft = maxAmmo - bulletPerMag;
        originalPos = transform.localPosition;
        InicialSpread = normalSpread;

        //Turn Units to Meters
        rangeUnit = (rangeMeters * 20) / 10;
    }

    // Update is called once per frame
    void Update()
    {
        //Fire Rate
        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        //Make Weapon Shoot
        if (fireMode == WeaponFireMode.Auto)
        {
            if (Input.GetButton("Fire1"))
            {
                if (IsReloading == false && currentBullets > 0)
                {
                    Shoot();
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
        else if (fireMode == WeaponFireMode.Semi)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (IsReloading == false && currentBullets > 0)
                {
                    Shoot();
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
        else if (fireMode == WeaponFireMode.Burst)
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
        if (currentBullets == 0 && currentBullets < bulletPerMag && bulletsLeft > 0)
        {
            StartCoroutine("ReloadFix");
        }
        
        //Aiming
        if (IsReloading == false)
        {
            ADS();
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * ADSSpeed);
            crosshair.SetActive(true);
        }
        
        //UpdateUI
        UpdateUI();
    }

    //Reload Function
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

    IEnumerator ReloadFix()
    {
        IsReloading = true;
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime);
        //audio.clip = reloadsound;
        //audio.Play();
        //anim.Stop("fire");
        //anim.Play("reload");
        yield return new WaitForSeconds(ReloadTime);
        ReloadFun();
        IsReloading = false;
    }

    //Shoot Function
    void Shoot()
    {
        if (fireTimer < fireRate || currentBullets <= 0) return;
        Debug.Log("Fired");

        RaycastHit hit;
        Vector3 direction = shootPoint.transform.forward;
        direction.x += Random.Range(-normalSpread, normalSpread);
        direction.y += Random.Range(-normalSpread, normalSpread);
        direction.z += Random.Range(-normalSpread, normalSpread);

        if (Physics.Raycast(shootPoint.position, direction, out hit, rangeUnit))
        {
            Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            Debug.Log("Hit " + hit.collider.name + " (" + hit.collider.tag + ")");

            if (hit.transform.tag == "Enemy")
            {

            }
        }
        
        //audio.clip = firesound;
        //audio.Play();

        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        currentBullets--;
        fireTimer = 0.0f;
    }

    //ADS Function
    void ADS()
    {
        if (Input.GetButton("Fire2"))
        {
            normalSpread = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsPos, Time.deltaTime * ADSSpeed);
            crosshair.SetActive(false);
        }
        else
        {
            normalSpread = InicialSpread;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * ADSSpeed);
            crosshair.SetActive(true);
        }
    }
    
    //Update UI
    void UpdateUI()
    {
        BulletsText.text = currentBullets.ToString() + "/" + bulletsLeft.ToString();
        WeaponNameText.text = WeaponName.ToString() + " - " + fireMode.ToString();
    }
}
