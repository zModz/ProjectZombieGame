using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJolt.API;

public class LogOut : MonoBehaviour {
    
    public void OnMouseClick()
    {
        var isSignedIn = GameJoltAPI.Instance.CurrentUser != null;
        if (isSignedIn)
        {
            GameJoltAPI.Instance.CurrentUser.SignOut();
        }
    }
}
