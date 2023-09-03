using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "pzg_weapon", menuName = "ProjectZombieGame/Create Weapon", order = 0)]
public class PzgWeapon : ScriptableObject {
    [FormerlySerializedAs("WeaponIndex")] [Header("Basic")]
    public int weaponIndex;
    [FormerlySerializedAs("WeaponName")] public string weaponName;
    [FormerlySerializedAs("WeaponIconUI")] public Sprite weaponIconUI;
    [FormerlySerializedAs("Wpn_GO")] public GameObject wpnGo;
    //public GameObject Arms_GO;
    public enum WeaponType { Assault, Smg, Lmg, Sniper, Shotgun, Pistol, Melee, Launcher, Special }
    public WeaponType weaponType;
    public enum WeaponFireMode { Auto, Semi, Burst, Bolt, Pump, Melee }
    public WeaponFireMode fireMode;

    //Weapon Atributes
    [Header("Weapon Atributes")]
    public AudioSource audio;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    [FormerlySerializedAs("SwayAmount")] public float swayAmount;
    [FormerlySerializedAs("SwaySmoothAmount")] public float swaySmoothAmount;
    [FormerlySerializedAs("SwayMaxAmount")] public float swayMaxAmount;
    public Vector3 adsPos;

    [Header("Weapon Stats")]
    public int bulletPerMag;
    public int maxAmmo;
    [FormerlySerializedAs("ReloadTime")] public float reloadTime;
    public int damage;
    [FormerlySerializedAs("ADSSpeed")] public float adsSpeed;
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
    [FormerlySerializedAs("WeaponIconBuy")] public Sprite weaponIconBuy;
    public int pointsToBuyAmmo;
    public int pointsToBuy;
}

