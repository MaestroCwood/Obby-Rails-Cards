using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public static ChatUI Instance;

    public TMP_InputField inputField;
    public GameObject messageObj;
    public GameObject ContentViewPort;
    public ScrollRect scrollRect;

    void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        ClearMessages();          // удаляем старые сообщения, если были
        StartCoroutine(LoadMessages());
    }

    private void OnEnable()
    {
        inputField.onSelect.AddListener(OnInputFieldSelected);
        inputField.onDeselect.AddListener(OnInputFieldDeselected);

        GameEvents.OnOpenChat += OnOpenChat;

    }



    private void OnDisable()
    {
        inputField.onSelect.RemoveListener(OnInputFieldSelected);
        inputField.onDeselect.RemoveListener(OnInputFieldDeselected);

        GameEvents.OnOpenChat -= OnOpenChat;

    }

    private void OnOpenChat()
    {
        
        Debug.Log("On OPEnchat!");
    }

    public void AddMessage(string msg)
    {
        GameObject go = Instantiate(messageObj, ContentViewPort.transform);
        go.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        ScrollToBottom();

    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
    public void OnSend()
    {
        string msg = inputField.text;
        if (string.IsNullOrWhiteSpace(msg)) return;

        // Add message locally immediately
        string displayText = $"<color=green>{ChatManager.Instance.UserName}</color>: {msg}";
        AddMessage(displayText);

        // Notify ChatManager that this message is pending (to avoid Photon echo duplication)
        ChatManager.Instance.AddPendingMessage(msg);

        ChatManager.Instance.SendMessageChaT(msg);
        StartCoroutine(SendMessageToDatabase(msg));

        inputField.text = "";
    }

    void OnInputFieldSelected(string value)
    {
        DeactivateControl();
    }

    void OnInputFieldDeselected(string value)
    {
        ActivateControl();
    }

    public void DeactivateControl()
    {
        Debug.Log("Управление отключено");
    }

    public void ActivateControl()
    {
        Debug.Log("Управление включено");
    }

    // Очистка всех сообщений из UI
    public void ClearMessages()
    {
        foreach (Transform child in ContentViewPort.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Загрузка последних сообщений из БД
    IEnumerator LoadMessages()
    {
     
        using (UnityWebRequest www = UnityWebRequest.Get("https://pixelartick.ru/chat-server/get_messages.php?chat=3"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load messages: " + www.error);
                yield break;
            }

            string json = www.downloadHandler.text;
            Debug.Log("Messages JSON: " + json);

            // Сервер возвращает массив объектов, например:
            // [{"id":1,"username":"user","message":"Hello","created_at":"..."}]
            // Для десериализации обернём его в объект с полем "messages"
            string wrappedJson = "{\"messages\":" + json + "}";
            MessageList messageList = JsonUtility.FromJson<MessageList>(wrappedJson);

            if (messageList != null && messageList.messages != null)
            {
                foreach (var msgData in messageList.messages)
                {
                    string displayText = $"<color=green>{msgData.username}</color>: {msgData.message}";
                    // Если нужно показывать имя пользователя:
                    // string displayText = msgData.username + ": " + msgData.message;
                    AddMessage(displayText);
                }
                Canvas.ForceUpdateCanvases();           // обновляем layout перед прокруткой
                scrollRect.verticalNormalizedPosition = 0f;
            }
            else
            {
                Debug.LogError("Failed to parse messages JSON");
            }
        }
    }

    // Отправка сообщения на сервер для сохранения в БД
    IEnumerator SendMessageToDatabase(string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("message", message);
        form.AddField("username", ChatManager.Instance.UserName);

       
        using (UnityWebRequest www = UnityWebRequest.Post("https://pixelartick.ru/chat-server/send_message.php?chat=3", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Message saved: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to save message: " + www.error);
            }
        }
    }

    // Классы для десериализации JSON
    [System.Serializable]
    public class MessageData
    {
        public int id;
        public string username;    // если в таблице есть поле username
        public string message;
        public string created_at;
    }

    [System.Serializable]
    public class MessageList
    {
        public MessageData[] messages;
    }
}