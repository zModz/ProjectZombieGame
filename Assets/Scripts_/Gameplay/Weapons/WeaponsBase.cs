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
    bool IsReloading = false;
    int fireMode;
    public Player_Movement plymove;
    Vector3 originalPos;
    [Header("UI")]
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI BulletsText;
    public Transform shootPoint;
    public bool isADS;

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = weapon.bulletPerMag;
        bulletsLeft = weapon.maxAmmo - weapon.bulletPerMag;
        originalPos = transform.localPosition;
        fireMode = (int)weapon.fireMode;
        
        //Turn Units to Meters
        rangeUnit = (weapon.rangeMeters * 20) / 10;
    }

    // Update is called once per frame
    void Update()
    {

        //UpdateUI();
        Inputs();
        Sway();

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

        //Make Weapon Shoot
        if (fireMode == 0)
        {
            if (Mouse.current.leftButton.isPressed /*|| plymove.gp1.rightTrigger.isPressed*/) // Mouse.current.leftButton.isPressed /*|| plymove.gp1.rightTrigger.isPressed*/
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
                    StartCoroutine("ReloadEnum");
                }
            }
        }
        else if (fireMode == 1)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame /*|| plymove.gp1.rightTrigger.wasPressedThisFrame*/)
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
                    StartCoroutine("ReloadEnum");
                }
            }
        }
        else if (fireMode == 2)
        {
            // Burst Fire
        }

        //ADS Function
        if (Mouse.current.rightButton.isPressed /*|| plymove.gp1.leftTrigger.isPressed*/)
        {
            isADS = true;
        }
        else
        {
            isADS = false;
        }
        ADS_Function();
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
    }

    IEnumerator ReloadEnum()
    {
        IsReloading = true;
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime);
        //audio.clip = reloadsound;
        //audio.Play();
        //anim.Stop("fire");
        //anim.Play("reload");
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
        if (IsReloading == false && currentBullets > 0)
        {
            if (fireTimer < weapon.fireRate || currentBullets <= 0) return;
            Debug.Log("Fired " + currentBullets);

            RaycastHit hit;
            Vector3 direction = shootPoint.transform.forward;

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
        else if (currentBullets <= 1 && bulletsLeft <= 0)
        {
            IsReloading = false;
        }
        else
        {
            StartCoroutine("ReloadEnum");
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
