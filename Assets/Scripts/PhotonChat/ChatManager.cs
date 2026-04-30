using Photon.Chat;
using Photon.Chat.DemoChat;
using System.Collections.Generic;
using UnityEngine;
//using YG;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public static ChatManager Instance;

    private ChatClient chatClient;
    private string userName;
    public string UserName => userName;
    private List<string> pendingLocalMessages = new List<string>();
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

       // userName = YG2.player.name;
        if (userName == "UNAUTHORIZED")
        {
           // if (YG2.envir.language == "ru")
            {
                userName = "Аноним";
            }
       //     else
            {
                userName = "anonymous";
            }
        }
        Init(userName);
    }

    public void Init(string playerName)
    {
        userName = playerName;

        chatClient = new ChatClient(this);

#if UNITY_WEBGL
        chatClient.TransportProtocol = ExitGames.Client.Photon.ConnectionProtocol.WebSocketSecure;
#endif

        chatClient.Connect(ChatSettings.Instance.AppId, "1.0", new AuthenticationValues(userName));
    }

    void Update()
    {
        chatClient?.Service();
    }

    public void SendMessageChaT(string msg)
    {
        chatClient.PublishMessage("global", msg);
    }

    public void OnConnected()
    {
        //chatClient.Subscribe(new string[] { "global" });
        string[] channels = { "global" };
        int[] lastMessages = { 50 };
        chatClient.Subscribe(channels, lastMessages);
        Debug.Log("OnConnected" + lastMessages[0]);
        GameEvents.OnOpenChat?.Invoke();
    }
    public void AddPendingMessage(string msg)
    {
        pendingLocalMessages.Add(msg);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            string sender = senders[i];
            string msg = messages[i].ToString();

            bool isLocal = (sender == userName);
            bool isPending = isLocal && pendingLocalMessages.Contains(msg);

            if (isPending)
            {
                // This is our own message that we already added locally – remove from pending and skip
                pendingLocalMessages.Remove(msg);
                continue;
            }

            string displayText = $"<color=green>{sender}</color>: {msg}";
            ChatUI.Instance.AddMessage(displayText);
        }

    }
    private void Reconnect()
    {
        if (chatClient != null)
        {
            chatClient.Connect(ChatSettings.Instance.AppId, "1.0", new AuthenticationValues(userName));
        }
    }

    // остальное можешь пока оставить пустым
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message) { }
    public void OnDisconnected()
    {
        Debug.Log("Chat disconnected, trying to reconnect...");
        // Попробуем переподключиться через 3 секунды
        Invoke(nameof(Reconnect), 3f);
    }
    public void OnChatStateChange(ChatState state) { }
    public void OnSubscribed(string[] channels, bool[] results) { }
    public void OnUnsubscribed(string[] channels) { }
    public void OnPrivateMessage(string sender, object message, string channelName) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }
}