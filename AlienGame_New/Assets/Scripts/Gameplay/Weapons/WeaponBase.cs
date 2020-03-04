using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Interactions;

public class WeaponBase : MonoBehaviour
{
    public WeaponManager Weapons;
    public Player_Script Player;

    AlienGame_New controls;

    //Weapon Atributes
    [Header("Weapon Atributes")]
    public int WeaponIndex;
    public Text WeaponNameText;
    public Text BulletsText;
    new AudioSource audio;
    public AudioClip firesound;
    public AudioClip reloadsound;
    public Transform shootPoint;
    float Sway_Amount;
    float Sway_smoothAmount;
    float Sway_maxAmount;
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
    public Image Weapon_Icon_Slot;
    
   
    // Start is called before the first frame update
    void Start()
    {
        Sway_Amount = 0.02f;
        Sway_maxAmount = 0.08f;
        Sway_smoothAmount = 6f;

        currentBullets = bulletPerMag;
        bulletsLeft = maxAmmo - bulletPerMag;
        originalPos = transform.localPosition;
        InicialSpread = normalSpread;

        //Turn Units to Meters
        rangeUnit = (rangeMeters * 20) / 10;

        Weapons = Weapons.GetComponent<WeaponManager>();
        Player = Player.GetComponent<Player_Script>();


    }

    // Update is called once per frame
    void Update()
    {
        Sway();
        foreach (var WeaponIndex in Weapons.WeaponList)
        {
            if (Weapons.WeaponList[this.WeaponIndex] == WeaponIndex)
            {
                Inputs();

                //UpdateUI
                UpdateUI();

                //Fire Rate
                if (fireTimer < fireRate)
                    fireTimer += Time.deltaTime;

                //Reload (Auto)
                if (currentBullets == 0 && currentBullets < bulletPerMag && bulletsLeft > 0)
                {
                    StartCoroutine("ReloadEnum");
                }
            }
        }
    }

    void Sway()
    {
        float moveX = -Input.GetAxis("Mouse X") * Sway_Amount;
        float moveY = -Input.GetAxis("Mouse Y") * Sway_Amount;
        moveX = Mathf.Clamp(moveX, -Sway_maxAmount, Sway_maxAmount);
        moveY = Mathf.Clamp(moveY, -Sway_maxAmount, Sway_maxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + originalPos, Time.deltaTime * Sway_smoothAmount);
    }

    void Inputs()
    {
        //Make Weapon Shoot
        if (Weapons.WeaponList[WeaponIndex].fireMode == weapons.WeaponFireMode.Auto)
        {
            if (Mouse.current.leftButton.isPressed)
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
        else if (Weapons.WeaponList[WeaponIndex].fireMode == weapons.WeaponFireMode.Semi)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
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
        else if (Weapons.WeaponList[WeaponIndex].fireMode == weapons.WeaponFireMode.Burst)
        {
            // Burst Fire
        }
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

    IEnumerator ReloadEnum()
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

    void Reload()
    {
        StartCoroutine("ReloadEnum");
    }

    //Shoot Function
    void Shoot()
    {
        //Make Weapon Shoot
        if (IsReloading == false && currentBullets > 0)
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
                    //Player
                    Player.GetPoints();
                }
            }

            //audio.clip = firesound;
            //audio.Play();

            if (fireTimer < fireRate)
                fireTimer += Time.deltaTime;

            currentBullets--;
            fireTimer = 0.0f;
        }
        else if (currentBullets <= 0 && bulletsLeft <= 0)
        {
            IsReloading = false;
        }
        else
        {
            StartCoroutine("ReloadEnum");
        }
    }

    //ADS Function
    void ADS()
    {
        if (Mouse.current.rightButton.isPressed)
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
        WeaponNameText.text = Weapons.WeaponList[WeaponIndex].name.ToString() + " - " + Weapons.WeaponList[WeaponIndex].fireMode.ToString();
        Weapon_Icon_Slot.sprite = Weapons.WeaponList[WeaponIndex].WeaponIcon_UI;
    }

    private void OnEnable()
    {
        //controls.Enable();
    }

    private void OnDisable()
    {
        //controls.Disable();
    }
}
