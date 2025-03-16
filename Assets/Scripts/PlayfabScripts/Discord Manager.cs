using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordManager : MonoBehaviour
{
    Discord.Discord discord;

    void Start()
    {
        discord = new Discord.Discord(1328464871741853727, (ulong)Discord.CreateFlags.NoRequireDiscord);
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    public void ChangeActivity(string state, string details){
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity{
            State = state,
            Details = details
        };
        activityManager.UpdateActivity(activity, (res) => {
            Debug.Log("discord activity changed");
        });
    }

    void OnDisable()
    {
        discord.Dispose();
    } 

    // defult statuses

    public void InMenu(){
        ChangeActivity("In Menu", "doing... menu things.");
    }

    public void InLobbySelecter(){
        ChangeActivity("In Lobby Selecter", "joining a lobby");
    }
}
