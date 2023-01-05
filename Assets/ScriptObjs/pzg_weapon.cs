using UnityEngine;

[CreateAssetMenu(fileName = "pzg_weapon", menuName = "ProjectZombieGame/Create Weapon", order = 0)]
public class pzg_weapon : ScriptableObject {
    [Header("Basic")]
    public int WeaponIndex;
    public string WeaponName;
    public Sprite WeaponIconUI;
    public GameObject Wpn_GO;
    //public GameObject Arms_GO;
    public enum WeaponType { Assault, SMG, LMG, Sniper, Shotgun, Pistol, Melee, Launcher, Special }
    public WeaponType weaponType;
    public enum WeaponFireMode { Auto, Semi, Burst, Bolt, Pump, Melee }
    public WeaponFireMode fireMode;

    //Weapon Atributes
    [Header("Weapon Atributes")]
    public AudioSource audio;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public float SwayAmount;
    public float SwaySmoothAmount;
    public float SwayMaxAmount;
    public Vector3 adsPos;

    [Header("Weapon Stats")]
    public int bulletPerMag;
    public int maxAmmo;
    public float ReloadTime;
    public int damage;
    public float ADSSpeed;
    public float rangeMeters;
    [Tooltip("<1 = Fast, >1 = Slow")]
    public float fireRate;
    [Header("Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snappiness;
    public float returnSpeed;
    //public float InicialSpread;
    [Header("Sniper/Shotgun Settings")]
    public float pumpSpeed;
    public float boltSpeed;

    [Header("Other Effects")]
    public GameObject crosshair;
    public GameObject bulletHole;
    public GameObject muzzleFlash;

    [Header("Buyable")]
    public bool isBuyable;
    public Sprite WeaponIconBuy;
    public int pointsToBuyAmmo;
    public int pointsToBuy;
}

