using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionAsset asset;

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
    public Transform groundCheck;
    public LayerMask groundMask;
    float groundDistance = 0.1f;
    float fallMulti = 2.5f;
    float lowMulti = 2f;
    Rigidbody rb;
    public bool isGrounded;
    public bool canJump;
    public bool isMoving;
    public bool isRunning;
    Collider[] childrenColliders;
    Vector3 velocity;

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

    private void Awake()
    {
        Char = GetComponent<CharacterController>();

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        childrenColliders = GetComponentsInChildren<Collider>();
        playerInput = GetComponent<PlayerInput>();
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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Movement
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        Char.Move(move * m_speed * Time.deltaTime);

        //Jump
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f);
        }

        //Gravity
        velocity.y += -9.81f * Time.deltaTime;
        Char.Move(velocity * Time.deltaTime);

        //Sprint
        if (move.z > 0)
        {
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame /*|| gp1.leftStickButton.wasPressedThisFrame*/ && isCrouching == false && isProne == false)
            {
                m_speed = m_Rspeed;
                isRunning = true;
            }
            else if (Keyboard.current.leftShiftKey.wasReleasedThisFrame /*|| gp1.leftStickButton.wasReleasedThisFrame*/)
            {
                m_speed = _m_speed;
                isRunning = false;
            }
        }

        Stances();

        if (move.z > 0 || move.z < 0 || move.x > 0 || move.x < 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void CamMove()
    {
        if (Time.deltaTime == 0) { return; }
        
        Vector2 camInput = playerInput.actions["Look"].ReadValue<Vector2>();

        //Cam Movement //Keyboard
        if (!isInverted)
        {
            var h = camInput.x * cam_sens_x * Time.deltaTime;
            v += camInput.y * -cam_sens_y * Time.deltaTime;

            //Rotate player with mouse input
            transform.Rotate(Vector3.up * h);

            //Rotate camera on the Y axis
            float xRotation = 0f; 
            xRotation += v;
            xRotation = Mathf.Clamp(v, -90, 90);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        else
        {
            var h = camInput.x * -cam_sens_x * Time.deltaTime;
            v += camInput.y * cam_sens_y * Time.deltaTime;

            //Rotate player with mouse input
            transform.Rotate(Vector3.up * h);

            //Rotate camera on the Y axis
            float xRotation = 0f;
            xRotation -= v;
            xRotation = Mathf.Clamp(v, -90, 90);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
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