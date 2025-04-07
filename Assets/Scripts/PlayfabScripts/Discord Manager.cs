#if !UNITY_STANDALONE_LINUX
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordManager : MonoBehaviour
{
    Discord.Discord discord;

    public bool discordRunning;

    string currentDetails;
    string currentState;

    void Start()
    {
        discord = new Discord.Discord(1328464871741853727, (ulong)Discord.CreateFlags.NoRequireDiscord);

        var activityManager = discord.GetActivityManager();

        if (activityManager == null) {
            discord.Dispose();
            Destroy(this);
        }
        else {
            discordRunning = true;
        }
    }

    void Update()
    {
        if (discordRunning){
            discord.RunCallbacks();
        }
    }

    public void ChangeActivity(string state, string details, string smallImageName, string smallImageText){
        var activityManager = discord.GetActivityManager();

        currentDetails = details;
        currentState = state;

        var activity = new Discord.Activity{
            State = state,
            Details = details,
            Assets = {
                SmallImage = smallImageName,
                SmallText = smallImageText
            }
        };
        activityManager.UpdateActivity(activity, (res) => {
            Debug.Log("discord activity changed");
        });
    }

    public void ChangeSmallImage(string smallImageName, string smallImageText){
        var activityManager = discord.GetActivityManager();

        var activity = new Discord.Activity{
            State = currentState,
            Details = currentDetails,
            Assets = {
                SmallImage = smallImageName,
                SmallText = smallImageText
            }
        };
        activityManager.UpdateActivity(activity, (res) => {
        });
    }

    void OnDisable()
    {
        discord.Dispose();
    } 

    // defult statuses

    public void InMenu(){
        if (discordRunning){
            ChangeActivity("In Menu", "doing... menu things.", null, null);
        }
    }

    public void InLobbySelecter(){
        if (discordRunning){
            ChangeActivity("In Lobby Selecter", "joining a lobby", null, null);
        }
    }

    public void InCustomizer(){
        if (discordRunning){
            ChangeActivity("In Goober Customization", "ooh, my goober looks fancy", null, null);
            ChangeSmallImageColor();
        }
    }

    // color changes in game
    public void ChangeSmallImageColor() {
        if (discordRunning) {
            string color = "color";
            int savedColor = PlayerPrefs.GetInt("color");

            if (savedColor == 0 || savedColor == 1){
                color = "green";
            }
            else if (savedColor == 2){
                color = "blue";
            }
            else if (savedColor == 3){
                color = "pink";
            }
            else if (savedColor == 4){
                color = "orange";
            }

            ChangeSmallImage(color, "current color: " + color);
        }
    }
}
# endif