using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    public GameObject cam;
    public int m_speed;
    [Range(0,100)]
    public int cam_sens_x;
    [Range(0, 100)]
    public int cam_sens_y;

    bool isMoving;
<<<<<<< Updated upstream
=======
    bool isRunning;
    Collider[] childrenColliders;

    //Crouch & Prone
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
>>>>>>> Stashed changes


    // Start is called before the first frame update
    void Start()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Screen.lockCursor = true;
#pragma warning restore CS0618 // Type or member is obsolete

<<<<<<< Updated upstream
=======
        //Crouch & Prone
        Char = GetComponent(typeof(CharacterController)) as CharacterController;
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
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //Char Movement
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * m_speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * m_speed;
        transform.Translate(x, 0, z);

        if (z > 0 || z < 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

<<<<<<< Updated upstream
        //Cam Movements
=======
        //Sprint
        if (z > 0)
        {
            if (Input.GetButtonDown("sprint"))
            {
                m_speed = m_Rspeed;
                isRunning = true;
            }
            else if (Input.GetButtonUp("sprint"))
            {
                m_speed = _m_speed;
                isRunning = false;
            }
        }

        //Cam Movement
        if (!isInverted)
        {
            var h = Input.GetAxis("Mouse X") * cam_sens_x;
            v += Input.GetAxis("Mouse Y") * -cam_sens_y;
            transform.Rotate(0, h, 0);

            v = Mathf.Clamp(v, -45, 90);
            cam.transform.eulerAngles = new Vector3(v, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        }
        else
        {
            var h = Input.GetAxis("Mouse X") * -cam_sens_x;
            v += Input.GetAxis("Mouse Y") * cam_sens_y;
            transform.Rotate(0, h, 0);

            v = Mathf.Clamp(v, -45, 90);
            cam.transform.eulerAngles = new Vector3(v, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        }
        

        //Jump
        if (canJump && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        //Crouch and Prone
        if (Input.GetKeyDown("c") && (isStanding == true))
        {
            Char.height = CrouchingHei;
            Char.center = CrouchingCenter;
            //cam.transform.position = new Vector3(cam.transform.position.x, CrouchPos, cam.transform.position.z);
            Crouch();
        }
        else if (Input.GetKeyDown("c") && (isCrouching == true))
        {
            Char.height = InicialHei;
            Char.center = InicialCenter;
            //cam.transform.position = new Vector3(cam.transform.position.x, Origpos, cam.transform.position.z);
            Stand();
        }
        else if ((Input.GetKeyDown("c")) && (isProne == true))
        {
            Char.height = InicialHei;
            Char.center = InicialCenter;
            CanGoProne = false;
            //cam.transform.position = new Vector3(cam.transform.position.x, Origpos, cam.transform.position.z);
            Stand();
        }


        if ((Input.GetKey("c")) && (CanGoProne == true))
        {
            Timing = 0.1f * Time.deltaTime;
            HoldTime += Timing;
            if (HoldTime >= GoProne)
            {
                Char.height = ProneHei;
                Char.center = ProneCenter;
                //cam.transform.position = new Vector3(cam.transform.position.x, PronePos, cam.transform.position.z);
                Prone();
            }
        }

        if (Input.GetKeyUp("c"))
        {
            HoldTime = 0.0f;
            if (CanGoProne == false)
            {
                CanGoProne = true;
            }
        }
    }

    void Jump()
    {
        onGround = false;
        if (onGround == false)
        {
            canJump = false;
        }
        rb.velocity = new Vector3(rb.velocity.x, CalculateJumpVerticalSpeed(), rb.velocity.z);
    }

    void Stand()
    {
        isStanding = true;
        isCrouching = false;
        isProne = false;
    }

    void Crouch()
    {
        isStanding = false;
        isCrouching = true;
        isProne = false;
    }

    void Prone()
    {
        isStanding = false;
        isCrouching = false;
        isProne = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    void DebugHUD()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * m_speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * m_speed;
>>>>>>> Stashed changes
        var h = Input.GetAxis("Mouse X") * cam_sens_x;
        var v = Input.GetAxis("Mouse Y") * -cam_sens_y;
        transform.Rotate(0, h, 0);

        v = Mathf.Clamp(v, -45, 90);
        cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x + v, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
    }
}
