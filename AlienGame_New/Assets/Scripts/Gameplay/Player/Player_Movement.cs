using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    Gamepad gp1;
    public string GP_Name;

    [Header("Player Atributtes")]
    public bool isInverted;
    bool isDown;
    bool wasDown;

    [Header("Character Atributtes")]
    public int m_speed;
    int _m_speed;
    public int runMulti;
    public int m_Rspeed;
    public float jumpHeight;
    float fallMulti = 2.5f;
    float lowMulti = 2f;
    Rigidbody rb;
    public bool canJump;
    public bool isMoving;
    public bool isRunning;
    Collider[] childrenColliders;

    [Header("Camera Atributtes")]
    public GameObject cam;
    public Camera weaponCam;
    [Range(0, 100)]
    public int cam_sens_x = 1;
    [Range(0, 100)]
    public int cam_sens_y = 1;
    public float v;

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
    public bool isStanding = false;
    public bool isCrouching = false;
    public bool isProne = false;
    float HoldTime = 0f;
    float GoProne = 0.08f;
    public bool CanGoProne = false;
    float Origpos;
    float tempPos;
    float CrouchPos;
    float PronePos;

    public void _Gamepad()
    {
        gp1 = Gamepad.current;
        if (gp1 == null)
            return;
    }

    private void Awake()
    {
        Char = GetComponent<CharacterController>();

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        childrenColliders = GetComponentsInChildren<Collider>();
        m_Rspeed = m_speed * runMulti;
        _m_speed = m_speed;


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

    }

    // Update is called once per frame
    void Update()
    {
        CamMove();
        Movement();

    }

    #region PlayerInput
    public void Movement()
    {
        _Gamepad();

        //Keyboard
        Vector3 move = new Vector3();

        if (Keyboard.current.wKey.isPressed) { move.z += 1; }
        if (Keyboard.current.aKey.isPressed) { move.x -= 1; }
        if (Keyboard.current.sKey.isPressed) { move.z -= 1; isRunning = false; }
        if (Keyboard.current.dKey.isPressed) { move.x += 1; }

        move.Normalize();

        transform.Translate(move * m_speed * Time.deltaTime);

        //Gamepad
        Vector3 gp_move = new Vector3();

        Vector2 gp = gp1.leftStick.ReadValue();
        gp_move.x += gp.x;
        gp_move.z += gp.y;

        gp_move.Normalize();

        transform.Translate(gp_move * m_speed * Time.deltaTime);





        //Sprint
        if (move.z > 0)
        {
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame || gp1.leftStickButton.wasPressedThisFrame && isCrouching == false && isProne == false)
            {
                m_speed = m_Rspeed;
                isRunning = true;
            }
            else if (Keyboard.current.leftShiftKey.wasReleasedThisFrame || gp1.leftStickButton.wasReleasedThisFrame)
            {
                m_speed = _m_speed;
                isRunning = false;
            }
        }

        Stances();
        if (Keyboard.current.spaceKey.wasPressedThisFrame || gp1.buttonSouth.wasPressedThisFrame)
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
        _Gamepad();

        //Cam Movement //Keyaboard
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

        //Cam Movement //Gamepad
        Vector2 gpcam = gp1.rightStick.ReadValue();
        if (!isInverted)
        {
            var h = gpcam.x * cam_sens_x;
            v += gpcam.y * -cam_sens_y;
            transform.Rotate(0, h, 0);

            v = Mathf.Clamp(v, -90, 75);
            cam.transform.eulerAngles = new Vector3(v, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        }
        else
        {
            var h = gpcam.x * -cam_sens_x;
            v += gpcam.y * cam_sens_y;
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
        else if (rb.velocity.y > 0 && !Keyboard.current.spaceKey.wasPressedThisFrame || !gp1.buttonSouth.wasPressedThisFrame)
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
}
