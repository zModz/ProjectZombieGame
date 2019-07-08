using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameJolt.API;

public class UserName : MonoBehaviour {
    
    public Text playerName;
    readonly bool isSignedIn = GameJoltAPI.Instance.HasSignedInUser;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        playerName.text = GameJoltAPI.Instance.CurrentUser.Name;
    }
}
