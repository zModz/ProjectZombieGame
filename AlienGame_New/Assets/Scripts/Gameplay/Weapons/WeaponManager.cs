using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct WeaponSettings
{
    public string name;

    [Header("Basic")]
    public int WeaponIndex;
    public string WeaponName;
    public Sprite WeaponIconUI;
    //public GameObject Wpn_GO;
    //public GameObject Arms_GO;
    public WeaponBase WpnBase;
    public enum WeaponType { Assault, SMG, LMG, Sniper, Shotgun, Pistol, Melee, Launcher }
    public WeaponType weaponType;
    public enum WeaponFireMode { Auto, Semi, Burst, Melee }
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
    [Tooltip("<1 = More Accurate, >1 = Less Accurate")]
    public float normalSpread;
    //public float InicialSpread;

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

[CreateAssetMenu(menuName = "WeaponSettings")]
public class WeaponManager : ScriptableObject
{
    [SerializeField]
    public WeaponSettings[] weaponList = new WeaponSettings[15];
}