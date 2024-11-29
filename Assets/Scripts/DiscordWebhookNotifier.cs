#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using UnityEngine;

[InitializeOnLoad]
public class DiscordWebhookNotifier
{
    private static string webhookUrl = "https://discord.com/api/webhooks/1312133353935343686/Ih9qLjCTTvEHzFSKClfBiKso-GrUl9mPFz-D7guUN8svoWN0Y7fovlIlBbo0A0i8bKH";

    static DiscordWebhookNotifier()
    {
        EditorApplication.quitting += OnEditorQuitting;
        SendMessageToDiscord($"Unity Project Opened by {System.Environment.UserName}");
    }

    [PostProcessScene]
    public static void OnPostProcessScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        SendMessageToDiscord($"Unity Project Opened by {System.Environment.UserName}");
        UnityEngine.Debug.Log("Opened");
    }

    static void OnEditorQuitting()
    {
        SendMessageToDiscord($"Unity Project Closed by {System.Environment.UserName}");
    }

    static async void SendMessageToDiscord(string message)
    {
        using (HttpClient client = new HttpClient())
        {
            var json = new StringContent(
                "{\"content\": \"" + message + "\"}", 
                Encoding.UTF8, 
                "application/json");

            await client.PostAsync(webhookUrl + "j", json);
        }
    }
}
#endif