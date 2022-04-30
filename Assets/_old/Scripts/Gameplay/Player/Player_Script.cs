using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI UI_Alert;
    public GameManager GameManager;
    public ScoreboardBehaviour scoreboard;

    [Header("Player Properties")]
    public GameObject GO_Scoreboard;
    //public GameObject GO_Pause;
    public float health;

    [HideInInspector]
    public int FPS { get; private set; }

    //Points
    [Header("Points")]
    public int Points;
    int pointsPerHit = 10;
    int InicialPoints = 500;
    public TextMeshProUGUI PointsText;

    [Header("Perk Manager")]
    public Image prkIcon;
    public Slider timerSlider;
    public PerksManager PrkBase;
    public int ActivePrk;
    float Timer;
    int DelayAmount = 1;


    // Start is called before the first frame update
    void Start()
    {
        //WpnBase = WpnBase.GetComponent<WeaponManager>();

        Cursor.lockState = CursorLockMode.Locked;
        GO_Scoreboard.SetActive(false);
        //GO_Pause.SetActive(false);

        //PlayerSetup
        scoreboard.p_name = GameManager.playerName;

        
        //Points = InicialPoints;
        PointsText.text = Points.ToString();

        //Perks

        /*/Write before this
        //Check which weapon is the current one
        WpnIdx = activeWeapons[currentWpn].WpnIndex;
        if (WpnIdx == Manager.weaponList[WpnIdx].WeaponIndex)
        {
            Weapons = Manager.weaponList[WpnIdx];
            SwitchWeapon(currentWpn);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //Character Atributtes

        //Player Properties
        ShowScoreboard();
        //ShowPause();
        PlayerLvl();

        //Points System
        PointsText.text = Points.ToString();

        //Weapons Systems
        //WeaponProperties();

        //Perks
        Perks();

        DebugHUD();
    }


    #region PlayerProperties
    void ShowScoreboard()
    {
        //plymove._Gamepad();

        if (Keyboard.current.tabKey.isPressed /*|| plymove.gp1.selectButton.isPressed*/)
        {
            GO_Scoreboard.SetActive(true);
        }
        else
        {
            GO_Scoreboard.SetActive(false);
        }
    }
    /*void ShowPause()
    {
        //plymove._Gamepad();

        if (Keyboard.current.escapeKey.wasPressedThisFrame && GO_Pause.active == false)
        {
            GO_Pause.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame && GO_Pause.active == true)
        {
            GO_Pause.SetActive(false);
            Time.timeScale = 1.0f;
        }

        if (GO_Pause.active == true) 
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else { Cursor.lockState = CursorLockMode.Locked; }
    }*/

    void PlayerLvl()
    {
        switch (GameManager.playerLvl)
        {
            case 1:
                scoreboard.p_lvl = GameManager.lvlIcons[0];
                break;
            case 10:
                scoreboard.p_lvl = GameManager.lvlIcons[1];
                break;
            case 20:
                scoreboard.p_lvl = GameManager.lvlIcons[2];
                break;
            case 30:
                scoreboard.p_lvl = GameManager.lvlIcons[3];
                break;
            case 40:
                scoreboard.p_lvl = GameManager.lvlIcons[4];
                break;
            case 50:
                scoreboard.p_lvl = GameManager.lvlIcons[5];
                break;
            case 60:
                scoreboard.p_lvl = GameManager.lvlIcons[6];
                break;
            case 70:
                scoreboard.p_lvl = GameManager.lvlIcons[7];
                break;
            case 80:
                scoreboard.p_lvl = GameManager.lvlIcons[8];
                break;
            case 90:
                scoreboard.p_lvl = GameManager.lvlIcons[9];
                break;
            case 100:
                scoreboard.p_lvl = GameManager.lvlIcons[10];
                break;
        }
    }


    public void GetPoints()
    {
        Points += pointsPerHit;
    }
    #endregion PlayerProperties

    #region Weapons
    /*void WeaponProperties()
    {
        //plymove._Gamepad();

        //Resize Weapon
        for (int i = 0; i < nrWpn; i++)
        {
            Array.Resize(ref activeWeapons, nrWpn);
        }

        //Switch
        for (int i = 1; i <= activeWeapons.Length; i++)
        {
            if(activeWeapons.Length > 1)
            {
                if (Input.GetKeyDown("" + i)/* || plymove.gp1.buttonNorth.wasPressedThisFrame)
                {
                    currentWpn = i - 1;

                    SwitchWeapon(currentWpn);
                }
            }
        }
    }

    public void SwitchWeapon(int index)
    {
        for (int i = 0; i < Manager.weaponList.Length; i++)
        {
            if (i == activeWeapons[index].WpnIndex)
            {
                Manager.weaponList[i].GO.gameObject.SetActive(true);
                //Weapons.GO.gameObject.SetActive(true);
            }
            else
            {
                Manager.weaponList[i].GO.gameObject.SetActive(false);
                //Weapons.GO.gameObject.SetActive(false);
            }
        }
    }*/
    #endregion Weapons

    #region Perks
    void Perks()
    {
        PrkBase.PerkList[ActivePrk].isActive = true;
        prkIcon.sprite = PrkBase.PerkList[ActivePrk].perkIcon;

        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            timerSlider.value++;
        }
        timerSlider.maxValue = PrkBase.PerkList[ActivePrk].timeEarn;
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