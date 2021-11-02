using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DiscordRichPresence : MonoBehaviour
{
    private static DiscordRichPresence _instance = null;
    public static DiscordRichPresence Instance
    {
        get { return _instance; }
    }

    DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

    public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
    DiscordRpc.EventHandlers handlers;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;

    private void Start()
    {
        UpdatePresence("In the menus", "");
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
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
    {
        Debug.Log(string.Format("Discord: connected to {0}#{1}: {2}", connectedUser.username, connectedUser.discriminator, connectedUser.userId));
        onConnect.Invoke();
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    void OnEnable()
    {
        Debug.Log("Discord: init");
        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback += ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        //handlers.joinCallback += JoinCallback;
        //handlers.spectateCallback += SpectateCallback;
        //handlers.requestCallback += RequestCallback;
        DiscordRpc.Initialize("668096972842074112", ref handlers, true, "");
    }

    void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }

    public void UpdatePresence(string state, string details)
    {
        presence.state = state;
        presence.details = details;
        presence.largeImageText = "Project: AlienGame";
        presence.largeImageKey = "weapons_collage";
        presence.smallImageText = "underdevelopement";
        presence.smallImageKey = "dc_logo_2";
        //presence.startTimestamp = (int)(DateTime.UtcNow - epochStart).TotalSeconds; //Can't make it work

        DiscordRpc.UpdatePresence(presence);
    }
}
