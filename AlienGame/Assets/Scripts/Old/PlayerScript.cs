using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    //PlayerHealth
    [Header("Player health")]
    public float Health = 100;
    public float MaxHealth = 100;
    public float regenHealthTimer = 5f;
    public Image DamageImg;
    bool isDead;
    public GameObject weaponManager;

    //PlayerMovment
    [Header("Player Movement")]
    public Camera Camera;
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
    float pos;

    //Points
    [Header("Points")]
    public int Points;
    int InicialPoints = 500;
    public Text PointsText;

    //Perks
    [Header("Perks")]
    public PerksManager[] PerkList = new PerksManager[3];

    // Use this for initialization
    void Start()
    {
        Char = GetComponent(typeof(CharacterController)) as CharacterController;
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
        Origpos = Camera.transform.position.y;
        pos = Camera.transform.position.y;

        Points = InicialPoints;
        PointsText.text = Points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Health
        if (Health < MaxHealth)
        {
            Health += regenHealthTimer * Time.deltaTime;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        //Points System
        PointsText.text = Points.ToString();

        //Controls (Crouch and Prone)
        if (Input.GetKeyDown("c") && (isStanding == true))
        {
            Char.height = CrouchingHei;
            Char.center = CrouchingCenter;
            pos = 0.343f;
            Crouch();
        }
        else if (Input.GetKeyDown("c") && (isCrouching == true))
        {
            Char.height = InicialHei;
            Char.center = InicialCenter;
            pos = Origpos;
            Stand();
        }
        else if ((Input.GetKeyDown("c")) && (isProne == true))
        {
            Char.height = InicialHei;
            Char.center = InicialCenter;
            CanGoProne = false;
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

    public void TakeDamageZombie(int amount)
    {
        // If the enemy is dead...
        if (isDead)
            return;

        Health -= amount;

        if (Health <= 0)
        {
            Prone();
            weaponManager.SetActive(false);
        }
    }
}
