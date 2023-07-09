using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    public PlayerInput playerInput;
    // private InputActionAsset asset;
    CharacterController Char;

    [Header("Player Atributtes")]
    public bool isInverted;
    // bool isDown;
    // bool wasDown;

    [Header("Character Atributtes")]
    public int m_speed;
    int _m_speed;
    public int runMulti;
    public int m_Rspeed;
    public float jumpHeight;
    public Transform groundCheck;
    public LayerMask groundMask;
    float groundDistance = 0.1f;
    // float fallMulti = 2.5f;
    // float lowMulti = 2f;
    Rigidbody rb;
    public bool isGrounded;
    public bool isMoving;
    public bool isRunning;
    Collider[] childrenColliders;
    Vector3 velocity;

    [Header("Camera Atributtes")]
    public GameObject cam;
    // public Camera weaponCam;
    [Range(0, 100)]
    public int cam_sens_x = 1;
    [Range(0, 100)]
    public int cam_sens_y = 1;
    public float v;
    public float h;

    [Header("Head Bobbing")]
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f; 
    //float defaultPosY = 0;
    //float timer = 0;

    private void Awake()
    {
        Char = GetComponent<CharacterController>();
        //defaultPosY = transform.localPosition.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
    }

    // Update is called once per frame
    void Update()
    {
        CamMove();
        Movement();
    }

    public void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        Char.Move(move * m_speed * Time.deltaTime);

        // Jump
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f);
        }

        // Gravity
        velocity.y += -9.81f * Time.deltaTime;
        Char.Move(velocity * Time.deltaTime);

        // Sprint
        if (input.y > 0)
        {
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame /*|| gp1.leftStickButton.wasPressedThisFrame*/)
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
        else if (input.y == 0)
        {
            if (Keyboard.current.leftShiftKey.isPressed)
            {
                m_speed = _m_speed;
                isRunning = false;
            }
        }


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

        // cam_sens_x *= 5;
        // cam_sens_y *= 5;
        
        Vector2 camInput = playerInput.actions["Look"].ReadValue<Vector2>();

        // Cam Movement // Keyboard
        if (!isInverted)
        {
            h = camInput.x * cam_sens_x * Time.deltaTime;
            v += camInput.y * -cam_sens_y * Time.deltaTime;

            // Rotate player with mouse input
            transform.Rotate(Vector3.up * h);

            // Rotate camera on the Y axis
            float xRotation = 0f; 
            xRotation += v;
            xRotation = Mathf.Clamp(v, -90, 90);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        else
        {
            var h = camInput.x * -cam_sens_x * Time.deltaTime;
            v += camInput.y * cam_sens_y * Time.deltaTime;

            // Rotate player with mouse input
            transform.Rotate(Vector3.up * h);

            // Rotate camera on the Y axis
            float xRotation = 0f;
            xRotation -= v;
            xRotation = Mathf.Clamp(v, -90, 90);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}