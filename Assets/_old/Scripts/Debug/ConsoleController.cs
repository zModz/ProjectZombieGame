using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConsoleController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;
    string input;
    Vector2 scroll;
    public GameManager gameManager;

    public static DebugCommand HELP;
    public static DebugCommand<int> LOAD_MAP;
    public static DebugCommand<int> SET_POINTS;
    public static DebugCommand<int> SET_AMMO;
    public static DebugCommand<int> SET_WEAPON;

    public List<object> commandList;

    private void Awake()
    {
        LOAD_MAP = new DebugCommand<int>("load_map", "Loads a choosen map", "load_map <map_index>", (x) =>
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(x);
        });

        HELP = new DebugCommand("help", "Shows all commands", "help", () => 
        {
            showHelp = true;
        }); 
        
        SET_POINTS = new DebugCommand<int>("set_points", "Sets a x amount of points", "set_points <points>", (x) =>
        {
            gameManager.player.Points = x;
        });

        SET_WEAPON = new DebugCommand<int>("set_weapon", "Sets the weapon curretly visable", "set_weapon <weapon_index>(0-14)", (x) =>
        {
            gameManager.weapons.SwitchWeapon(x);
        });


        commandList = new List<object>
        {
            HELP,
            LOAD_MAP,
            SET_POINTS,
            SET_WEAPON
        };
    }


    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            if (Keyboard.current.insertKey.wasPressedThisFrame)
            {
                showConsole = !showConsole;
            }

            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                if (showConsole)
                {
                    HandleInput();
                    input = "";
                }
            }
        }
    }

    private void OnGUI()
    {
        if(!showConsole) { return; }

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commnadDesc}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();
            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 30);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    void HandleInput()
    {
        string[] prop = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandbase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandbase.commandId))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(prop[1]));
                }
            }
        }
    }
}
