using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using System;

public class DiscordController : MonoBehaviour
{
    Discord.Discord discord = new Discord.Discord(668096972842074112, (UInt64)CreateFlags.NoRequireDiscord);

    private void Start()
    {
        UpdateActivity("", "Menus");
    }

    private void Update()
    {
        discord.RunCallbacks();
    }

    public void UpdateActivity(string state, string details)
    {
        var activityManager = discord.GetActivityManager();
        var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        var activity = new Activity
        {
            State = state,
            Details = details,
            Timestamps =
            {
                Start = Timestamp,
            },
            Assets =
            {
                LargeImage = "bg_largeimage",
                LargeText = "Project: AlienGame",
                SmallImage = "dc_logo_2",
                SmallText = "under development",
            },
            Instance = true,
        };

        activityManager.UpdateActivity(activity, (result) =>
            {
                if (result == Result.Ok)
                {
                    Debug.Log("Success!");
                }
                else
                {
                    Debug.Log("Failed");
                }
            }
        );
    }

    private void OnDisable()
    {
        discord.Dispose();
    }
}
