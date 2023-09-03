using System;
using System.Collections;
using System.Collections.Generic;
using Scripts_.Gameplay.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

[System.Serializable]
public struct ActivePerks
{
    [FormerlySerializedAs("PrkIndex")] public int prkIndex;
}

public class PlayerScript : MonoBehaviour
{
    [FormerlySerializedAs("DebugTXT")] [SerializeField]
    Text debugTxt;
    public PlayerMovement plymove;
    public CameraMovement camMove;
    [FormerlySerializedAs("UI_Alert")] public TextMeshProUGUI uiAlert;
    //public GameManager GameManager;
    //public ScoreboardBehaviour scoreboard;

    [FormerlySerializedAs("GO_Scoreboard")] [Header("Player Properties")]
    public GameObject goScoreboard;
    //public GameObject GO_Pause;
    public float health;

    [HideInInspector]
    public int FPS { get; private set; }

    //Points
    [FormerlySerializedAs("Points")] [Header("Points")]
    public int points;
    int _pointsPerHit = 10;
    // int InicialPoints = 500;
    [FormerlySerializedAs("PointsText")] public TextMeshProUGUI pointsText;


    // Start is called before the first frame update
    void Start()
    {
        //WpnBase = WpnBase.GetComponent<WeaponManager>();

        Cursor.lockState = CursorLockMode.Locked;
        goScoreboard.SetActive(false);
        //GO_Pause.SetActive(false);

        //PlayerSetup
        //scoreboard.p_name = GameManager.playerName;

        
        //Points = InicialPoints;
        pointsText.text = points.ToString();

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
        //PlayerLvl();

        //Points System
        pointsText.text = points.ToString();

        //Weapons Systems
        //WeaponProperties();

        //Perks
        //Perks();

        DebugHUD();
    }


    #region PlayerProperties
    void ShowScoreboard()
    {
        //plymove._Gamepad();

        if (Keyboard.current.tabKey.isPressed /*|| plymove.gp1.selectButton.isPressed*/)
        {
            goScoreboard.SetActive(true);
        }
        else
        {
            goScoreboard.SetActive(false);
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

    // void PlayerLvl()
    // {
    //     switch (GameManager.playerLvl)
    //     {
    //         case 1:
    //             scoreboard.p_lvl = GameManager.lvlIcons[0];
    //             break;
    //         case 10:
    //             scoreboard.p_lvl = GameManager.lvlIcons[1];
    //             break;
    //         case 20:
    //             scoreboard.p_lvl = GameManager.lvlIcons[2];
    //             break;
    //         case 30:
    //             scoreboard.p_lvl = GameManager.lvlIcons[3];
    //             break;
    //         case 40:
    //             scoreboard.p_lvl = GameManager.lvlIcons[4];
    //             break;
    //         case 50:
    //             scoreboard.p_lvl = GameManager.lvlIcons[5];
    //             break;
    //         case 60:
    //             scoreboard.p_lvl = GameManager.lvlIcons[6];
    //             break;
    //         case 70:
    //             scoreboard.p_lvl = GameManager.lvlIcons[7];
    //             break;
    //         case 80:
    //             scoreboard.p_lvl = GameManager.lvlIcons[8];
    //             break;
    //         case 90:
    //             scoreboard.p_lvl = GameManager.lvlIcons[9];
    //             break;
    //         case 100:
    //             scoreboard.p_lvl = GameManager.lvlIcons[10];
    //             break;
    //     }
    // }


    public void GetPoints()
    {
        points += _pointsPerHit;
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
    // void Perks()
    // {
    //     PrkBase.PerkList[ActivePrk].isActive = true;
    //     prkIcon.sprite = PrkBase.PerkList[ActivePrk].perkIcon;

    //     Timer += Time.deltaTime;

    //     if (Timer >= DelayAmount)
    //     {
    //         Timer = 0f;
    //         timerSlider.value++;
    //     }
    //     timerSlider.maxValue = PrkBase.PerkList[ActivePrk].timeEarn;
    // }
    #endregion Perks

    void DebugHUD()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * plymove.walkSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * plymove.walkSpeed;
        var h = Input.GetAxis("Mouse X") * camMove.camSensX;
        FPS = (int)(1f / Time.deltaTime);

        debugTxt.text = 
            " - " + "FPS: " + FPS + " frames" +
            "\n - " + "Can Jump: " + plymove.isGrounded +
            "\n - " + "Is On Ground: " + /*plymove.isGrounded +*/
            "\n - " + "Is Moving: " + plymove.isMoving + " @ " + plymove.walkSpeed + " speed" +
            "\n - " + "Is Running: " + plymove.isRunning + " @ " + plymove.runningSpeed + " speed" +
            "\n - " + "Is Inverted: " + plymove.isInverted +
            "\n - " + "X Values: " + x +
            " - " + "Z Values: " + z +
            "\n - " + "H Values: " + h +
            " - " + "V Values: " + camMove.v;
    }
}