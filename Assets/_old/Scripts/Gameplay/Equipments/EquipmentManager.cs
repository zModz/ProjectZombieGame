using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lethal
{
    public string Name;
    public Sprite EquipIcon_UI;
    public GameObject GO;

    [Header("Buyable")]
    public bool isBuyable;
    public Sprite WeaponIcon_buy;
    public int pointsToBuy;
    public int pointsToBuyAmmo;
}

[System.Serializable]
public class Tactical
{
    public string Name;
    public Sprite EquipIcon_UI;
    public GameObject GO;

}
public class EquipmentManager : MonoBehaviour
{
    public Lethal[] LethalList = new Lethal[3];
    public Tactical[] TacticalList = new Tactical[3];
}
