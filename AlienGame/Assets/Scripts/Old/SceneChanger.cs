using UnityEngine;
using GameJolt.API;

public class SceneChanger : MonoBehaviour {

    public bool UserHasSignIn;
    Sprite userAvatar = GameJoltAPI.Instance.CurrentUser.Avatar;
    public GameObject menu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseClick()
	{
        if (!UserHasSignIn)
        {
            GameJolt.UI.GameJoltUI.Instance.ShowSignIn(
                (bool signInSuccess) => 
                {
                    if (signInSuccess == true)
                    {
                        UserHasSignIn = true;
                        Debug.Log(string.Format("Sign-in {0}", signInSuccess ? "successful" : "failed or user's dismissed the window"));
                        GameJolt.UI.GameJoltUI.Instance.QueueNotification(GameJoltAPI.Instance.CurrentUser.Name + " Sign in");
                        menu.SetActive(true);
                    }
                });
        }
        else
        {
            menu.SetActive(true);
        }



	}
}
