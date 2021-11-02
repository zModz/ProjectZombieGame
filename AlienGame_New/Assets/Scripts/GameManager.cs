using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using PlayFab;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Player_Script player;
    public WeaponSwitch weapons;
    public PlayfabManager manager;

    [Header("Player")]
    public bool isLoggedIn;
    public Sprite[] lvlIcons = new Sprite[11];
    [Range(1, 100)]
    public int playerLvl;
    public string playerName;

    [Header("Maps")]
    public maps[] MapsList = new maps[5];

    [Header("Loadouts")]
    public WeaponManager Manager;
    public int startingWeapon = 9;

    [Header("SystemInfo")]
    string CPU;
    string coreCount;
    string frequencyNum;
    string RAM;
    string GPU;
    string VRAM;
    string gpuVersion;
    string OS;

    private void Start()
    {
        //PlayFab
        manager = GetComponent<PlayfabManager>();

        //SystemInfo
        CPU = SystemInfo.processorType.ToString();
        coreCount = SystemInfo.processorCount.ToString();
        frequencyNum = SystemInfo.processorFrequency.ToString();
        RAM = SystemInfo.systemMemorySize.ToString();
        GPU = SystemInfo.graphicsDeviceName.ToString();
        gpuVersion = SystemInfo.graphicsDeviceVersion.ToString();
        VRAM = SystemInfo.graphicsMemorySize.ToString();
        OS = SystemInfo.operatingSystem.ToString();

        Debug.Log("CPU: " + CPU + " " + coreCount + " Cores at " + frequencyNum + "Mhz" + 
        "\n" + "RAM: " + RAM + 
        "\n" + "GPU: " + GPU + " " + VRAM + "MB " + gpuVersion + 
        "\n" + "OS: " + OS);
    }


    private void Update()
    {

        if (playerName == "")
        {
            playerName = manager.name;
        }

        if (manager.LoggedIn)
        {
            isLoggedIn = true;
        }
    }
}
