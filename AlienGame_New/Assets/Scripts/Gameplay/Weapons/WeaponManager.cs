using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class weapons
{
    [Header("Basic")]
    public string name;
    public Sprite WeaponIcon_UI;
    public GameObject GO;
    public WeaponBase WpnBase;
    public enum WeaponType { Assault, SMG, LMG, Sniper, Shotgun, Pistol, Melee, Launcher }
    public WeaponType weaponType;
    public enum WeaponFireMode { Auto, Semi, Burst, Melee }
    public WeaponFireMode fireMode;

    [Header("Buyable")]
    public bool isBuyable;
    public Sprite WeaponIcon_buy;
    public int pointsToBuy;
    public int pointsToBuyAmmo;
}

public class WeaponManager : MonoBehaviour
{
    public weapons[] WeaponList = new weapons[3];
}