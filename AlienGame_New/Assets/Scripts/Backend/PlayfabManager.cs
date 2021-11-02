using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class PlayfabManager : MonoBehaviour
{
    [Header("Register")]
    public InputField r_email;
    public InputField r_password;
    //public InputField r_cpassword;
    [Header("Login")]
    public InputField l_email;
    public InputField l_password;
    [Header("ResetPassword")]
    public InputField reset_email;
    [Header("displayName")]
    public InputField displayName;
    public GameObject displayNameWindow;
    public GameObject loginregisterWindow;
    [Header("Booleans")]
    public bool ready;
    public bool LoggedIn;
    [Header(" ")]
    public string name;
    public Text message;

    public void registerButton()
    {
        if (r_password.text.Length < 6) { message.text = "Password too short!"; return; }
        //if(password.text == cpassword.text) { return; } else { message.text = "Passwords arent' the same"; }

        var request = new RegisterPlayFabUserRequest
        {
            Email = r_email.text,
            Password = r_password.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = l_email.text,
            Password = l_password.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucess, OnError);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = reset_email.text,
            TitleId = "ECD54"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    public void SubmitDisplayName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        message.text = "Registered!, you may now login.";
    }

    void OnLoginSucess(LoginResult result)
    {
        Debug.Log("Logged In");
        message.text = "Logged in.";
        name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;

        if (name == null)
        {
            displayNameWindow.SetActive(true);
        }
        else
        {
            loginregisterWindow.SetActive(false);
            ready = true;
        }

        LoggedIn = true;
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        message.text = "Password reset mail sent!";
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        loginregisterWindow.SetActive(false);
        displayNameWindow.SetActive(false);
        ready = true;
    }

    void OnError(PlayFabError error) 
    {
        message.text = error.ErrorMessage + "(" + error.ErrorDetails + ")";
    }
}
