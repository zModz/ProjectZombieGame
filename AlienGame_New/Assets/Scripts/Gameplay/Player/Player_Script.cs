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

    [Header("Player Properties")]
    public GameObject GO_Scoreboard;
    public float health;
    bool isDown;
    bool wasDown;

    [Header("Player Atributtes")]
    public bool isInverted;
    public Text UI_Alert;
    
    [Header("Camera Atributtes")]
    public GameObject cam;
    public Camera weaponCam;
    [Range(0, 100)]
    public int cam_sens_x;
    [Range(0, 100)]
    public int cam_sens_y;
    float v;
    [HideInInspector]
    public int FPS { get; private set; }

    [Header("Character Atributtes")]
    public int m_speed;
    int _m_speed;
    public int runMulti;
    int m_Rspeed;
    public float jumpHeight;
    float fallMulti = 2.5f;
    float lowMulti = 2f;
    Rigidbody rb;
    bool canJump;
    bool isMoving;
    bool isRunning;
    Collider[] childrenColliders;
    AlienGame_New controls;
    
    //Crouch & Prone
    [Header("Crouch & Prone")]
    public Sprite[] stance_icns = new Sprite[3];
    public Image stanceIcn;
    public int c_speed;
    public int p_speed;
    CharacterController Char;
    Vector3 InicialPos;
    float InicialHei;
    float CrouchingHei;
    float ProneHei;
    Vector3 InicialCenter;
    Vector3 CrouchingCenter;
    Vector3 ProneCenter;
    float Timing = 0.0f;
    bool isStanding = false;
    bool isCrouching = false;
    bool isProne = false;
    float HoldTime = 0f;
    float GoProne = 0.08f;
    bool CanGoProne = false;
    float Origpos;
    float tempPos;
    float CrouchPos;
    float PronePos;

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
        Char = GetComponent<CharacterController>();
        controls = new AlienGame_New();
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable() => controls.Disable();
    

    // Start is called before the first frame update
    void Start()
    {
        WpnBase = WpnBase.GetComponent<WeaponManager>();

        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        childrenColliders = GetComponentsInChildren<Collider>();
        m_Rspeed = m_speed * runMulti;
        _m_speed = m_speed;
        GO_Scoreboard.SetActive(false);

        foreach (Collider col in childrenColliders)
        {
            // checking if it is our collider, then skip it, 
            if (col != GetComponent<Collider>())
            {
                // if it is not our collider then ignore collision between our collider and childs collider
                Physics.IgnoreCollision(col, GetComponent<Collider>());
            }
        }
        
        //Crouch & Prone
        CrouchPos = 1f;
        PronePos = 0.5f;
        InicialHei = Char.height;
        CrouchingHei = InicialHei - 0.5f;
        ProneHei = InicialHei - 1.4f;
        InicialCenter = Char.center;
        CrouchingCenter = InicialCenter + Vector3.down * 0.25f;
        ProneCenter = InicialCenter + Vector3.down * 0.6f;
        isStanding = true;
        isCrouching = false;
        isProne = false;
        CanGoProne = true;
        Origpos = cam.transform.position.y;
        tempPos = Origpos;

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
        CamMove();
        Movement();

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

    #region PlayerInput
    public void Movement()
    {
        Vector3 move = new Vector3();

        if (Keyboard.current.wKey.isPressed) { move.z += 1; }
        if (Keyboard.current.aKey.isPressed) { move.x -= 1; }
        if (Keyboard.current.sKey.isPressed) { move.z -= 1; isRunning = false; }
        if (Keyboard.current.dKey.isPressed) { move.x += 1; }

        move.Normalize();

        transform.Translate(move * m_speed * Time.deltaTime);
        
        //Sprint
        if (move.z > 0)
        {
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame && isCrouching == false && isProne == false)
            {
                m_speed = m_Rspeed;
                isRunning = true;
            }
            else if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
            {
                m_speed = _m_speed;
                isRunning = false;
            }
        }

        Stances();
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector3.up * jumpHeight;
        }
        Jump();

        if (move.z > 0 || move.z < 0 || move.x > 0 || move.x < 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    void Stances()
    {
        //Crouch and Prone
        if (Keyboard.current.cKey.wasPressedThisFrame && (isStanding == true))
        {
            Char.height = CrouchingHei;
            Char.center = CrouchingCenter;
            cam.transform.position = new Vector3(cam.transform.position.x, CrouchPos, cam.transform.position.z);
            Crouch();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame && (isCrouching == true))
        {
            Char.height = InicialHei;
            Char.center = InicialCenter;
            cam.transform.position = new Vector3(cam.transform.position.x, Origpos, cam.transform.position.z);
            Stand();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame && (isProne == true))
        {
            Char.height = InicialHei;
            Char.center = InicialCenter;
            CanGoProne = false;
            cam.transform.position = new Vector3(cam.transform.position.x, Origpos, cam.transform.position.z);
            Stand();
        }


        if (Keyboard.current.cKey.isPressed && (CanGoProne == true))
        {
            Timing = 0.1f * Time.deltaTime;
            HoldTime += Timing;
            if (HoldTime >= GoProne)
            {
                Char.height = ProneHei;
                Char.center = ProneCenter;
                cam.transform.position = new Vector3(cam.transform.position.x, PronePos, cam.transform.position.z);
                Prone();
            }
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            HoldTime = 0.0f;
            if (CanGoProne == false)
            {
                CanGoProne = true;
            }
        }
    }
    
    public void CamMove()
    {
        //Cam Movement
        if (!isInverted)
        {
            var h = Input.GetAxis("Mouse X") * cam_sens_x;
            v += Input.GetAxis("Mouse Y") * -cam_sens_y;
            transform.Rotate(0, h, 0);

            v = Mathf.Clamp(v, -90, 75);
            cam.transform.eulerAngles = new Vector3(v, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        }
        else
        {
            var h = Input.GetAxis("Mouse X") * -cam_sens_x;
            v += Input.GetAxis("Mouse Y") * cam_sens_y;
            transform.Rotate(0, h, 0);

            v = Mathf.Clamp(v, -90, 75);
            cam.transform.eulerAngles = new Vector3(v, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        }
    }

    void Jump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMulti - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMulti - 1) * Time.deltaTime;
        }
    }
    
    void Stand()
    {
        m_speed = _m_speed;
        stanceIcn.sprite = stance_icns[0];
        weaponCam.fieldOfView = 60.0f;
        isStanding = true;
        isCrouching = false;
        isProne = false;
    }

    void Crouch()
    {
        m_speed = c_speed;
        stanceIcn.sprite = stance_icns[1];
        weaponCam.fieldOfView = 50.0f;
        isStanding = false;
        isCrouching = true;
        isProne = false;
    }

    void Prone()
    {
        m_speed = p_speed;
        stanceIcn.sprite = stance_icns[2];
        weaponCam.fieldOfView = 45.0f;
        isStanding = false;
        isCrouching = false;
        isProne = true;
        if (isDown)
        {
            //Change icon to a "isDown"
        }
    }
    #endregion

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
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * m_speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * m_speed;
        var h = Input.GetAxis("Mouse X") * cam_sens_x;
        FPS = (int)(1f / Time.deltaTime);

        DebugTXT.text = 
            " - " + "FPS: " + FPS + " frames" +
            "\n - " + "Can Jump: " + canJump +
            "\n - " + "Is On Ground: " + Char.isGrounded +
            "\n - " + "Is Moving: " + isMoving + " @ " + m_speed + " speed" +
            "\n - " + "Is Running: " + isRunning + " @ " + m_Rspeed + " speed" +
            "\n - " + "Is Inverted: " + isInverted +
            "\n - " + "X Values: " + x +
            " - " + "Z Values: " + z +
            "\n - " + "H Values: " + h +
            " - " + "V Values: " + v +
            "\n - " + "isStanding: " + isStanding +
            " - " + "isCrouching: " + isCrouching +
            "\n - " + "isProne: " + isProne +
            " - " + "CanGoProne: " + CanGoProne;
    }
}