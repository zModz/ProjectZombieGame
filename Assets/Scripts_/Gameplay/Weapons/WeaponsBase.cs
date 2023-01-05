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
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    [Header("UI")]
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI BulletsText;
    public Transform shootPoint;

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
        //Sway();

        // Recoil
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, weapon.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, weapon.snappiness * Time.fixedDeltaTime);
        plymove.cam.transform.localRotation = Quaternion.Euler(currentRotation);

        // Fire Rate
        if (fireTimer < weapon.fireRate)
            fireTimer += Time.deltaTime;

        // Reload (Auto)
        if (currentBullets == 0 && currentBullets < weapon.bulletPerMag && bulletsLeft > 0)
        {
            StartCoroutine("ReloadEnum");
        }
    }

    //void Sway()
    //{
    //    Vector2 camInput = plymove.playerInput.actions["Look"].ReadValue<Vector2>();

    //    float moveX = -camInput.x * weapon.SwayAmount;
    //    float moveY = -camInput.y * weapon.SwayAmount;
    //    moveX = Mathf.Clamp(moveX, -weapon.SwayMaxAmount, weapon.SwayMaxAmount);
    //    moveY = Mathf.Clamp(moveY, -weapon.SwayMaxAmount, weapon.SwayMaxAmount);

    //    Vector3 finalPos = new Vector3(moveX, moveY, 0);
    //    transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + originalPos, Time.deltaTime * weapon.SwaySmoothAmount);
    //}

    void Inputs()
    {
        if (Time.deltaTime == 0){ return; }
        //plymove._Gamepad();

        //Make Weapon Shoot
        if (fireMode == 0)
        {
            if (Mouse.current.leftButton.isPressed /*|| plymove.gp1.rightTrigger.isPressed*/)
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

        /*/ADS Function
        if (Mouse.current.rightButton.isPressed || plymove.gp1.leftTrigger.isPressed)
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
        }*/
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
            Debug.Log("Fired");

            RaycastHit hit;
            Vector3 direction = shootPoint.transform.forward;

            targetRotation += new Vector3(weapon.recoilX, Random.Range(-weapon.recoilY, weapon.recoilY), Random.Range(-weapon.recoilZ, weapon.recoilZ));

            if (Physics.Raycast(shootPoint.position, direction, out hit, rangeUnit))
            {
                Instantiate(weapon.bulletHole, hit.point, Quaternion.FromToRotation(Vector3.back, hit.normal));
                Debug.Log("Hit " + hit.collider.name + " (" + hit.collider.tag + ")");

                if (hit.transform.tag == "Enemy")
                {
                    //Player
                    //Player.GetPoints();
                }
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
