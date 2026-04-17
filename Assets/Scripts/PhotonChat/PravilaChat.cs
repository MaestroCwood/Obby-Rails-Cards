using TMPro;
using UnityEngine;
using YG;

public class PravilaChat : MonoBehaviour
{
    const string CHAT_CHECK = "ChatCheck";
   
    [SerializeField] TextMeshProUGUI TextContent;
    string language;
    private void Start()
    {   
        language = YG2.envir.language;
        Debug.Log(language);
        TextContent.text = SetText();
        if (PlayerPrefs.GetInt(CHAT_CHECK) == 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }


    }

    public void SaveCheck()
    {
        PlayerPrefs.SetInt(CHAT_CHECK, 1);
        PlayerPrefs.Save();
    }

    string SetText()
    {
        string ru = "1. <color=#FF5555><b>Запрещено</b></color> использовать нецензурную лексику и оскорблять других участников.\r\n2. " +
            "<color=#FFAA00><b>Не допускается</b></color> флуд (частое повторение сообщений, спам, бессмысленные тексты).\r\n3." +
            " <color=#FF5555><b>Запрещена</b></color> реклама сторонних ресурсов, серверов и услуг без разрешения администрации.\r\n4. " +
            "<color=#55FF55><b>Соблюдайте</b></color> уважительное общение и не провоцируйте конфликты.\r\n5. " +
            "<color=#55AAFF><b>Выполняйте</b></color> указания администрации — нарушение правил может привести к " +
            "<color=#FF0000><b>блокировке</b></color>.\r\n";
        string en = "1. <color=#FF5555><b>Forbidden</b></color> to use profanity and insult other participants.\r\n2. " +
        "<color=#FFAA00><b>Not allowed</b></color> flooding (repeated messages, spam, meaningless text).\r\n3." +
        " <color=#FF5555><b>Forbidden</b></color> advertising third-party resources, servers, or services without admin permission.\r\n4. " +
        "<color=#55FF55><b>Maintain</b></color> respectful communication and do not provoke conflicts.\r\n5. " +
        "<color=#55AAFF><b>Follow</b></color> the instructions of the administration — violations may result in " +
        "<color=#FF0000><b>ban</b></color>.\r\n";

        return language == "ru" ? ru : en;
    }
}
