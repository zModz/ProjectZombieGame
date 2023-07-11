using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponsBase : MonoBehaviour
{
    public pzg_weapon weapon;
    int currentBullets;
    int bulletsLeft;
    float fireTimer;
    float rangeUnit;
    int fireMode;
    public Player_Movement plymove;
    public Animator anim;
    int isMovingHash;
    int isRunningHash;
    int isShootingHash;
    int isReloadingHash;
    int isReloadingFullHash;
    int ReloadMulti;
    int fireRate;
    Vector3 originalPos;

    [Header("UI")]
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI BulletsText;
    public Transform shootPoint;
    public bool IsReloading = false;
    public bool isADS;

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = weapon.bulletPerMag;
        bulletsLeft = weapon.maxAmmo - weapon.bulletPerMag;
        originalPos = transform.localPosition;
        fireMode = (int)weapon.fireMode;

        // Animations 
        anim = GetComponent<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
        isRunningHash = Animator.StringToHash("isRunning");
        isShootingHash = Animator.StringToHash("isShooting");
        isReloadingHash = Animator.StringToHash("isReloading");
        isReloadingFullHash = Animator.StringToHash("isReloadingFull");
        ReloadMulti = Animator.StringToHash("ReloadMulti");
        fireRate = Animator.StringToHash("FireRate");

        //Turn Units to Meters
        rangeUnit = (weapon.rangeMeters * 20) / 10;
    }

    // Update is called once per frame
    void Update()
    {

        //UpdateUI();
        Inputs();
        Sway();
        Animations();

        // Fire Rate
        if (fireTimer < weapon.fireRate)
            fireTimer += Time.deltaTime;

        // Reload (Auto)
        if (currentBullets == 0 && currentBullets < weapon.bulletPerMag && bulletsLeft > 0)
        {
            StartCoroutine("ReloadEnum");
        }
    }

    void Recoil()
    {
        // Recoil
        Debug.Log("recoiled");

        float currRecoilPosX = ((Random.value - .5f) /2) * weapon.recoilX;
        float currRecoilPosY = ((Random.value - .5f) / 2) * (fireTimer >= 4 ? weapon.recoilY / 4 : weapon.recoilY);

        plymove.h -= Mathf.Abs(currRecoilPosY);
        plymove.v -= currRecoilPosX;
    }

    void Sway()
    {
        Vector2 camInput = plymove.playerInput.actions["Look"].ReadValue<Vector2>();

        float moveX = -camInput.x * weapon.SwayAmount;
        float moveY = -camInput.y * weapon.SwayAmount;
        moveX = Mathf.Clamp(moveX, -weapon.SwayMaxAmount, weapon.SwayMaxAmount);
        moveY = Mathf.Clamp(moveY, -weapon.SwayMaxAmount, weapon.SwayMaxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + originalPos, Time.deltaTime * weapon.SwaySmoothAmount);
    }

    void Inputs()
    {
        if (Time.deltaTime == 0){ return; }
        //plymove._Gamepad();

        // Make Weapon Shoot
        if (fireMode == 0)
        {
            if (plymove.playerInput.actions["fire"].IsPressed())
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
                    Reload();
                }
            }
        }
        else if (fireMode == 1)
        {
            if (plymove.playerInput.actions["fire"].WasPressedThisFrame())
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
                    Reload();
                }
            }
        }
        else if (fireMode == 2)
        {
            // Burst Fire
        }

        // ADS Function
        if (plymove.playerInput.actions["ads"].IsPressed())
        {
            isADS = true;
        }
        else
        {
            isADS = false;
        }
        ADS_Function();

        // Manual Reload
        if (plymove.playerInput.actions["reload"].WasPressedThisFrame())
        {
            Reload();
        }
    }

    void ADS_Function()
    {
        if (!isADS)
        {
            //normalSpread = InicialSpread;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * weapon.ADSSpeed);
            //crosshair.SetActive(true);
        }
        else
        {
            //normalSpread = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, weapon.adsPos, Time.deltaTime * weapon.ADSSpeed);
            //crosshair.SetActive(false);
        }
    }

    //Reload Function
    void ReloadFun()
    {
        if (IsReloading == true)
        {
            if (bulletsLeft <= 1) return;

            int bulletToLoad = weapon.bulletPerMag - currentBullets;
            int bulletsToDeduct = (bulletsLeft >= bulletToLoad) ? bulletToLoad : bulletsLeft;

            bulletsLeft -= bulletsToDeduct;
            currentBullets += bulletsToDeduct;
        }

        Debug.Log("reloading...");
    }

    IEnumerator ReloadEnum()
    {
        IsReloading = true;
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime);
        //audio.clip = reloadsound;
        //audio.Play();
        yield return new WaitForSeconds(weapon.ReloadTime);
        ReloadFun();
        IsReloading = false;
    }

    void Reload()
    {
        StartCoroutine("ReloadEnum");
    }

    //Shoot Function
    public void Shoot()
    {
        //Make Weapon Shoot
        if (fireTimer < weapon.fireRate || currentBullets <= 0) return;
        Debug.Log("Fired");

        RaycastHit hit;
        Vector3 direction = shootPoint.transform.forward;


        //plymove.m_speed = plymove._m_speed;
        //plymove.isRunning = false;
        Recoil();

        if (Physics.Raycast(shootPoint.position, direction, out hit, rangeUnit))
        {
            // Create a bullet hole in the object the player just shot
            // // TODO: Have a diferent bullet hole for each material (for realism sake)
            Instantiate(weapon.bulletHole, hit.point, Quaternion.FromToRotation(Vector3.back, hit.normal), hit.transform);

            // Logic in case the player hits an enemy
            if (hit.transform.tag == "Enemy")
            {
                //Player
                //Player.GetPoints();
            }

            // Logic in case the player hits a object with physics
            // // STILL DECIDING IF THIS MAKES INTO FINAL PRODUCT
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForceAtPosition(250 * direction, hit.point);
            }





            // Debug shit
            Debug.Log("Hit " + hit.collider.name + " (" + hit.collider.tag + ")");
        }

        //audio.clip = firesound;
        //audio.Play();

        if (fireTimer < weapon.fireRate)
            fireTimer += Time.deltaTime;

        currentBullets--;
        fireTimer = 0.0f;
    }

    void Animations()
    {
        bool isMoving = anim.GetBool(isMovingHash);
        bool isRunning = anim.GetBool(isRunningHash);
        bool isShooting = anim.GetBool(isShootingHash);
        bool isReloading = anim.GetBool(isReloadingHash);
        bool isReloadingFull = anim.GetBool(isReloadingFullHash);

        //float reloadMulti = weapon.ReloadTime * 1;
        //anim.SetFloat(ReloadMulti, reloadMulti);

        //float firerate = weapon.fireRate * 6f;
        //anim.SetFloat(fireRate, firerate);

        if (!isMoving && !isShooting && plymove.isMoving) {
            anim.SetBool(isMovingHash, true);
        }

        if(isMoving && !plymove.isMoving)
        {
            anim.SetBool(isMovingHash, false);
        }

        if (!isRunning && !isShooting && (plymove.isMoving && plymove.isRunning))
        {
            anim.SetBool(isRunningHash, true);
        }

        if (isRunning && (!plymove.isMoving || !plymove.isRunning))
        {
            anim.SetBool(isRunningHash, false);
        }




        if (!isShooting && isMoving && isRunning && fireTimer < weapon.fireRate)
        {
            anim.SetBool(isMovingHash, false);
            anim.SetBool(isRunningHash, false);
            anim.SetBool(isShootingHash, true);
        }

        if (isShooting && !(fireTimer < weapon.fireRate))
        {
            anim.SetBool(isShootingHash, false);
        }





        if (!isReloading && isMoving && isShooting && currentBullets > 0 && currentBullets < weapon.bulletPerMag && IsReloading)
        {
            anim.SetBool(isMovingHash, false);
            anim.SetBool(isRunningHash, false);
            anim.SetBool(isShootingHash, false);
            anim.SetBool(isReloadingHash, true);
        }
        else if (!isReloadingFull && currentBullets == 0 && IsReloading)
        {
            anim.SetBool(isMovingHash, false);
            anim.SetBool(isRunningHash, false);
            anim.SetBool(isShootingHash, false);
            anim.SetBool(isReloadingFullHash, true);
        }

        if (isReloading && !IsReloading)
        {
            anim.SetBool(isReloadingHash, false);
        }
        else if(isReloadingFull && !IsReloading)
        {
            anim.SetBool(isReloadingFullHash, false);
        }
    }

    //Update UI
    void UpdateUI()
    {
        BulletsText.text = currentBullets.ToString() + "/" + bulletsLeft.ToString();
        WeaponNameText.text = weapon.WeaponName.ToString() + " - " + weapon.fireMode.ToString();
        //Weapon_Icon_Slot.sprite = weapon.WeaponIconUI;
    }
}
