using Photon.Chat;
using Photon.Chat.DemoChat;
using UnityEngine;
using YG;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public static ChatManager Instance;

    private ChatClient chatClient;
    private string userName;
    public string UserName => userName;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       
        userName = YG2.player.name;
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

    public void SendMessage(string msg)
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
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            //string text = senders[i] + ": " + messages[i];
            string text = $"<color=green>{senders[i]}</color>: {messages[i]}";

           
            ChatUI.Instance.AddMessage(text);
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