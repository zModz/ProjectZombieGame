using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts_.Gameplay.Player;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Serialization;

public class WeaponsBase : MonoBehaviour
{
    public PzgWeapon weapon;
    int _currentBullets;
    int _bulletsLeft;
    float _fireTimer;
    float _rangeUnit;
    int _fireMode;
    public PlayerMovement plymove;
    public CameraMovement camMove;
    public Animator anim;
    int _isMovingHash;
    int _isRunningHash;
    int _isShootingHash;
    int _isReloadingHash;
    int _isReloadingFullHash;
    int _reloadMulti;
    int _fireRate;
    Vector3 _originalPos;

    [FormerlySerializedAs("WeaponNameText")] [Header("UI")]
    public TextMeshProUGUI weaponNameText;
    [FormerlySerializedAs("BulletsText")] public TextMeshProUGUI bulletsText;
    public Transform shootPoint;
    [FormerlySerializedAs("IsReloading")] public bool isReloading = false;
    [FormerlySerializedAs("isADS")] public bool isAds;

    // Start is called before the first frame update
    void Start()
    {
        _currentBullets = weapon.bulletPerMag;
        _bulletsLeft = weapon.maxAmmo - weapon.bulletPerMag;
        _originalPos = transform.localPosition;
        _fireMode = (int)weapon.fireMode;

        // Animations 
        anim = GetComponent<Animator>();
        _isMovingHash = Animator.StringToHash("isMoving");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isShootingHash = Animator.StringToHash("isShooting");
        _isReloadingHash = Animator.StringToHash("isReloading");
        _isReloadingFullHash = Animator.StringToHash("isReloadingFull");
        _reloadMulti = Animator.StringToHash("ReloadMulti");
        _fireRate = Animator.StringToHash("FireRate");

        //Turn Units to Meters
        _rangeUnit = (weapon.rangeMeters * 20) / 10;
    }

    // Update is called once per frame
    void Update()
    {

        //UpdateUI();
        Inputs();
        Sway();
        Animations();

        // Fire Rate
        if (_fireTimer < weapon.fireRate)
            _fireTimer += Time.deltaTime;

        // Reload (Auto)
        if (_currentBullets == 0 && _currentBullets < weapon.bulletPerMag && _bulletsLeft > 0)
        {
            StartCoroutine("ReloadEnum");
        }
    }

    void Recoil()
    {
        // Recoil
        Debug.Log("recoiled");

        float currRecoilPosX = ((Random.value - .5f) /2) * weapon.recoilX;
        float currRecoilPosY = ((Random.value - .5f) / 2) * (_fireTimer >= 4 ? weapon.recoilY / 4 : weapon.recoilY);

        camMove.h -= Mathf.Abs(currRecoilPosY);
        camMove.v -= currRecoilPosX;
    }

    void Sway()
    {
        Vector2 camInput = plymove.playerInput.actions["Look"].ReadValue<Vector2>();

        float moveX = -camInput.x * weapon.swayAmount;
        float moveY = -camInput.y * weapon.swayAmount;
        moveX = Mathf.Clamp(moveX, -weapon.swayMaxAmount, weapon.swayMaxAmount);
        moveY = Mathf.Clamp(moveY, -weapon.swayMaxAmount, weapon.swayMaxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + _originalPos, Time.deltaTime * weapon.swaySmoothAmount);
    }

    void Inputs()
    {
        if (Time.deltaTime == 0){ return; }
        //plymove._Gamepad();

        // Make Weapon Shoot
        if (_fireMode == 0)
        {
            if (plymove.playerInput.actions["fire"].IsPressed())
            {
                if (isReloading == false && _currentBullets > 0)
                {
                    Shoot();
                }
                else if (_currentBullets <= 0 && _bulletsLeft <= 0)
                {
                    isReloading = false;
                }
                else
                {
                    Reload();
                }
            }
        }
        else if (_fireMode == 1)
        {
            if (plymove.playerInput.actions["fire"].WasPressedThisFrame())
            {
                if (isReloading == false && _currentBullets > 0)
                {
                    Shoot();
                }
                else if (_currentBullets <= 0 && _bulletsLeft <= 0)
                {
                    isReloading = false;
                }
                else if (_currentBullets == 0)
                {
                    Reload();
                }
            }
        }
        else if (_fireMode == 2)
        {
            // Burst Fire
        }

        // ADS Function
        if (plymove.playerInput.actions["ads"].IsPressed())
        {
            isAds = true;
        }
        else
        {
            isAds = false;
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
        if (!isAds)
        {
            //normalSpread = InicialSpread;
            transform.localPosition = Vector3.Lerp(transform.localPosition, _originalPos, Time.deltaTime * weapon.adsSpeed);
            //crosshair.SetActive(true);
        }
        else
        {
            //normalSpread = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, weapon.adsPos, Time.deltaTime * weapon.adsSpeed);
            //crosshair.SetActive(false);
        }
    }

    //Reload Function
    void ReloadFun()
    {
        if (isReloading == true)
        {
            if (_bulletsLeft <= 1) return;

            int bulletToLoad = weapon.bulletPerMag - _currentBullets;
            int bulletsToDeduct = (_bulletsLeft >= bulletToLoad) ? bulletToLoad : _bulletsLeft;

            _bulletsLeft -= bulletsToDeduct;
            _currentBullets += bulletsToDeduct;
        }

        Debug.Log("reloading...");
    }

    IEnumerator ReloadEnum()
    {
        isReloading = true;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _originalPos, Time.deltaTime);
        //audio.clip = reloadsound;
        //audio.Play();
        yield return new WaitForSeconds(weapon.reloadTime);
        ReloadFun();
        isReloading = false;
    }

    void Reload()
    {
        StartCoroutine("ReloadEnum");
    }

    //Shoot Function
    public void Shoot()
    {
        //Make Weapon Shoot
        if (_fireTimer < weapon.fireRate || _currentBullets <= 0) return;
        Debug.Log("Fired");

        RaycastHit hit;
        Vector3 direction = shootPoint.transform.forward;


        //plymove.m_speed = plymove._m_speed;
        //plymove.isRunning = false;
        Recoil();

        if (Physics.Raycast(shootPoint.position, direction, out hit, _rangeUnit))
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

        if (_fireTimer < weapon.fireRate)
            _fireTimer += Time.deltaTime;

        _currentBullets--;
        _fireTimer = 0.0f;
    }

    void Animations()
    {
        bool isMoving = anim.GetBool(_isMovingHash);
        bool isRunning = anim.GetBool(_isRunningHash);
        bool isShooting = anim.GetBool(_isShootingHash);
        bool isReloading = anim.GetBool(_isReloadingHash);
        bool isReloadingFull = anim.GetBool(_isReloadingFullHash);

        //float reloadMulti = weapon.ReloadTime * 1;
        //anim.SetFloat(ReloadMulti, reloadMulti);

        //float firerate = weapon.fireRate * 6f;
        //anim.SetFloat(fireRate, firerate);

        if (!isMoving && !isShooting && plymove.isMoving) {
            anim.SetBool(_isMovingHash, true);
        }

        if(isMoving && !plymove.isMoving)
        {
            anim.SetBool(_isMovingHash, false);
        }

        if (!isRunning && !isShooting && (plymove.isMoving && plymove.isRunning))
        {
            anim.SetBool(_isRunningHash, true);
        }

        if (isRunning && (!plymove.isMoving || !plymove.isRunning))
        {
            anim.SetBool(_isRunningHash, false);
        }




        if (!isShooting && isMoving && isRunning && _fireTimer < weapon.fireRate)
        {
            anim.SetBool(_isMovingHash, false);
            anim.SetBool(_isRunningHash, false);
            anim.SetBool(_isShootingHash, true);
        }

        if (isShooting && !(_fireTimer < weapon.fireRate))
        {
            anim.SetBool(_isShootingHash, false);
        }





        if (!isReloading && isMoving && isShooting && _currentBullets > 0 && _currentBullets < weapon.bulletPerMag && this.isReloading)
        {
            anim.SetBool(_isMovingHash, false);
            anim.SetBool(_isRunningHash, false);
            anim.SetBool(_isShootingHash, false);
            anim.SetBool(_isReloadingHash, true);
        }
        else if (!isReloadingFull && _currentBullets == 0 && this.isReloading)
        {
            anim.SetBool(_isMovingHash, false);
            anim.SetBool(_isRunningHash, false);
            anim.SetBool(_isShootingHash, false);
            anim.SetBool(_isReloadingFullHash, true);
        }

        if (isReloading && !this.isReloading)
        {
            anim.SetBool(_isReloadingHash, false);
        }
        else if(isReloadingFull && !this.isReloading)
        {
            anim.SetBool(_isReloadingFullHash, false);
        }
    }

    //Update UI
    void UpdateUI()
    {
        bulletsText.text = _currentBullets.ToString() + "/" + _bulletsLeft.ToString();
        weaponNameText.text = weapon.weaponName.ToString() + " - " + weapon.fireMode.ToString();
        //Weapon_Icon_Slot.sprite = weapon.WeaponIconUI;
    }
}
