using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public struct ActiveWeapon
{
    public int WpnIndex;
}

[System.Serializable]
public struct ActivePerks
{
    public int PrkIndex;
}

public class Player_Script : MonoBehaviour
{
    [SerializeField]
    Text DebugTXT;
    public Player_Movement plymove;
    public Text UI_Alert;

    [Header("Player Properties")]
    public GameObject GO_Scoreboard;
    public float health;

    [HideInInspector]
    public int FPS { get; private set; }

    //Points
    [Header("Points")]
    public int Points;
    int pointsPerHit = 10;
    int InicialPoints = 500;
    public Text PointsText;

    [Header("Score")]
    public int Score;
    int scorePerKill = 100;
    public Slider ScoreSlider;


    [Header("Weapon Manager")]
    public WeaponManager WpnBase;
    public int currentWpn;
    [Range(1, 3)]
    public int nrWpn;
    int WpnIdx;
    public ActiveWeapon[] activeWeapons = new ActiveWeapon[0];

    [Header("Perk Manager")]
    public PerksManager PrkBase;
    public GameObject[] PrkSlots;
    [Range(1, 3)]
    public int nrPrk;
    public ActivePerks[] activePrk = new ActivePerks[0];



    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        WpnBase = WpnBase.GetComponent<WeaponManager>();

        Cursor.lockState = CursorLockMode.Locked;
        GO_Scoreboard.SetActive(false);

        
        //Points = InicialPoints;
        PointsText.text = Points.ToString();

        //Perks
        Perks();

        //Write before this
        //Check which weapon is the current one
        foreach (var WeaponIndex in WpnBase.WeaponList)
        {
            WpnIdx = activeWeapons[currentWpn].WpnIndex;
            if (WpnBase.WeaponList[WpnIdx] == WeaponIndex)
            {
                SwitchWeapon(currentWpn);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Character Atributtes

        //Player Properties
        ShowScoreboard();

        //Points System
        PointsText.text = Points.ToString();

        //Weapons Systems
        WeaponProperties();

        //Perks
       if (PrkSlots[0].GetComponent<PerkSlotBehaviour>().isActive)
        {
            ChangePrkScore(1);
            return;
        }
       else if (PrkSlots[1].GetComponent<PerkSlotBehaviour>().isActive)
        {
            ChangePrkScore(2);
            return;
        }
        
        DebugHUD();
    }


    #region PlayerProperties
    void ShowScoreboard()
    {
        if (Keyboard.current.tabKey.isPressed)
        {
            GO_Scoreboard.SetActive(true);
        }
        else
        {
            GO_Scoreboard.SetActive(false);
        }
    }

    public void GetScore()
    {
        ScoreSlider.value += scorePerKill;
    }

    public void GetPoints()
    {
        Points += pointsPerHit;
    }
    #endregion PlayerProperties

    

    #region Weapons
    void WeaponProperties()
    {
        //Resize Weapon
        for (int i = 0; i < nrWpn; i++)
        {
            Array.Resize(ref activeWeapons, nrWpn);
        }

        //Switch
        for (int i = 1; i <= activeWeapons.Length; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                currentWpn = i - 1;

                SwitchWeapon(currentWpn);
            }
        }
    }

    public void SwitchWeapon(int index)
    {
        for (int i = 0; i < WpnBase.WeaponList.Length; i++)
        {
            if (i == index)
            {

                WpnBase.WeaponList[activeWeapons[i].WpnIndex].WpnBase.gameObject.SetActive(true);
            }
            else
            {
                try
                {
                    WpnBase.WeaponList[activeWeapons[i].WpnIndex].WpnBase.gameObject.SetActive(false);
                }
                catch (IndexOutOfRangeException) { };
            }
        }
    }
    #endregion Weapons

    #region Perks
    void Perks()
    {
        for (int i = 0; i < nrPrk; i++)
        {
            Array.Resize(ref activePrk, nrPrk);
        }

        switch (nrPrk)
        {
            default:
                break;
            case 1:
                PrkSlots[0].gameObject.active = true;
                PrkSlots[1].gameObject.active = false;
                PrkSlots[2].gameObject.active = false;
                break;
            case 2:
                PrkSlots[0].gameObject.active = true;
                PrkSlots[1].gameObject.active = true;
                PrkSlots[2].gameObject.active = false;
                break;
            case 3:
                PrkSlots[0].gameObject.active = true;
                PrkSlots[1].gameObject.active = true;
                PrkSlots[2].gameObject.active = true;
                break;
        }
        
        ChangePrkScore(0);
    }

    void ChangePrkScore(int prkSlot)
    {
        Debug.Log("Value Chnaged?!: " + prkSlot);
        ScoreSlider.value = 0;
        ScoreSlider.maxValue = PrkSlots[prkSlot].GetComponent<PerkSlotBehaviour>().scoreToGet;
    }
    #endregion Perks

    void DebugHUD()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * plymove.m_speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * plymove.m_speed;
        var h = Input.GetAxis("Mouse X") * plymove.cam_sens_x;
        FPS = (int)(1f / Time.deltaTime);

        DebugTXT.text = 
            " - " + "FPS: " + FPS + " frames" +
            "\n - " + "Can Jump: " + plymove.canJump +
            "\n - " + "Is On Ground: " + /*plymove.isGrounded +*/
            "\n - " + "Is Moving: " + plymove.isMoving + " @ " + plymove.m_speed + " speed" +
            "\n - " + "Is Running: " + plymove.isRunning + " @ " + plymove.m_Rspeed + " speed" +
            "\n - " + "Is Inverted: " + plymove.isInverted +
            "\n - " + "X Values: " + x +
            " - " + "Z Values: " + z +
            "\n - " + "H Values: " + h +
            " - " + "V Values: " + plymove.v +
            "\n - " + "isStanding: " + plymove.isStanding +
            " - " + "isCrouching: " + plymove.isCrouching +
            "\n - " + "isProne: " + plymove.isProne +
            " - " + "CanGoProne: " + plymove.CanGoProne;
    }
}