using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponBase : MonoBehaviour
{
    int currentBullets;
    int bulletsLeft;
    float fireTimer;
    Vector3 originalPos;
    Quaternion originalRot;
    float rangeUnit;
    GameObject go;
 
    [Header("Weapon Settings")]
    public int wpn_index;
    public WeaponManager Manager;
    WeaponSettings Weapons;
    public Player_Script Player;
    public Player_Movement plymove;
    [SerializeField]
    bool IsReloading = false;

    [Header("UI")]
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI BulletsText;
    public Transform shootPoint;


    void Check()
    {
        if (wpn_index == Manager.weaponList[wpn_index].WeaponIndex)
        {
            Weapons = Manager.weaponList[wpn_index];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Check();

        currentBullets = Weapons.bulletPerMag;
        bulletsLeft = Weapons.maxAmmo - Weapons.bulletPerMag;
        originalPos = transform.localPosition;
        originalRot = transform.rotation;
        //Weapons.InicialSpread = normalSpread;

        //Turn Units to Meters
        rangeUnit = (Weapons.rangeMeters * 20) / 10;

        Player = Player.GetComponent<Player_Script>();
        go = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update
        Check();
        UpdateUI();
        Inputs();
        Sway();

        //Fire Rate
        if (fireTimer < Weapons.fireRate)
            fireTimer += Time.deltaTime;

        //Reload (Auto)
        if (currentBullets == 0 && currentBullets < Weapons.bulletPerMag && bulletsLeft > 0)
        {
            StartCoroutine("ReloadEnum");
        }
    }

    void Sway()
    {
        Vector2 camInput = plymove.playerInput.actions["Look"].ReadValue<Vector2>();

        float moveX = -camInput.x * Weapons.SwayAmount;
        float moveY = -camInput.y * Weapons.SwayAmount;
        moveX = Mathf.Clamp(moveX, -Weapons.SwayMaxAmount, Weapons.SwayMaxAmount);
        moveY = Mathf.Clamp(moveY, -Weapons.SwayMaxAmount, Weapons.SwayMaxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + originalPos, Time.deltaTime * Weapons.SwaySmoothAmount);
    }

    void Inputs()
    {
        if (Time.deltaTime == 0){ return; }
        //plymove._Gamepad();

        //Make Weapon Shoot
        if (Weapons.fireMode == WeaponSettings.WeaponFireMode.Auto)
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
        else if (Weapons.fireMode == WeaponSettings.WeaponFireMode.Semi)
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
        else if (Weapons.fireMode == WeaponSettings.WeaponFireMode.Burst)
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

            int bulletToLoad = Weapons.bulletPerMag - currentBullets;
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
        yield return new WaitForSeconds(Weapons.ReloadTime);
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
            if (fireTimer < Weapons.fireRate || currentBullets <= 0) return;
            Debug.Log("Fired");

            RaycastHit hit;
            Vector3 direction = shootPoint.transform.forward;
            direction.x += Random.Range(-Weapons.normalSpread, Weapons.normalSpread);
            direction.y += Random.Range(-Weapons.normalSpread, Weapons.normalSpread);
            direction.z += Random.Range(-Weapons.normalSpread, Weapons.normalSpread);

            //Visual Recoil
            ///Weapon Force - Position
            Vector3 vRecoilPos = new Vector3(0f, 0f, 0.1f);
            transform.localPosition = Vector3.Lerp(-vRecoilPos, transform.localPosition, 10 * Time.deltaTime);
            Debug.Log(vRecoilPos + " / " + transform.localPosition);

            ///Weapon Force - Rotation
            


            if (Physics.Raycast(shootPoint.position, direction, out hit, rangeUnit))
            {
                Instantiate(Weapons.bulletHole, hit.point, Quaternion.FromToRotation(Vector3.back, hit.normal));
                Debug.Log("Hit " + hit.collider.name + " (" + hit.collider.tag + ")");

                if (hit.transform.tag == "Enemy")
                {
                    //Player
                    Player.GetPoints();
                }
            }

            //audio.clip = firesound;
            //audio.Play();

            if (fireTimer < Weapons.fireRate)
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
        Check();
        BulletsText.text = currentBullets.ToString() + "/" + bulletsLeft.ToString();
        WeaponNameText.text = Weapons.WeaponName.ToString() + " - " + Weapons.fireMode.ToString();
        //Weapon_Icon_Slot.sprite = Weapons.WeaponIconUI;
    }
}
