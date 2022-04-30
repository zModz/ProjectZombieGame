using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeScrnsht : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.f10Key.wasPressedThisFrame)
        {
            string date = DateTime.Now.ToString();
            date = date.Replace("/", "-");
            date = date.Replace(" ", "_");
            date = date.Replace(":", ".");
            string appName = Application.productName.ToString();
            appName = appName.Replace(": ", "_");
            string resolution = Screen.currentResolution.ToString();
            resolution = resolution.Replace(" ", "");
            string text = appName + "_Screenshot_" + resolution + "_" + date + ".png";
            string path = Application.persistentDataPath + "/screenshots/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            ScreenCapture.CaptureScreenshot(path + text);
            Debug.Log("Screenshot Saved.");
        }
    }
}
