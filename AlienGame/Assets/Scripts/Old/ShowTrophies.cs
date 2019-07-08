using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJolt.API;

public class ShowTrophies : MonoBehaviour {

    public void OnMouseClick()
    {
        //GameJolt.UI.GameJoltUI.Instance.ShowTrophies();
        Application.OpenURL("https://gamejolt.com/@" + GameJoltAPI.Instance.CurrentUser.Name);
    }
	
}
