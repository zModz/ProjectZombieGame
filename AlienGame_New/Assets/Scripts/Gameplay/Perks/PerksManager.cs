using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Perks
{
    [Header("Basic")]
    public string Name;
    public bool isActive;
    public Sprite perkIcon;
    public GameObject Activity;

    //Removed
    [Header("Buyable")]
    public bool isBuyable;
    public int timeEarn;
    //Removed
}

public class PerksManager : MonoBehaviour
{
    public Perks[] PerkList = new Perks[3];
}
