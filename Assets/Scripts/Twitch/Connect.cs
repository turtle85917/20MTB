using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class Connect : MonoBehaviour
{
    [SerializeField] private string nick;
    [SerializeField] private string username;
    [SerializeField] private string oauth;
    [SerializeField] private GameObject Chat;
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    private void Awake()
    {
        twitchClient = new("irc.chat.twitch.tv", 6667);
        reader = new(twitchClient.GetStream());
        writer = new(twitchClient.GetStream());
    }

    private void Start()
    {
        writer.WriteLine("PASS " + oauth);
        writer.WriteLine("NICK " + nick);
        writer.WriteLine("USER " + username + " 8 * : " + username);
        writer.WriteLine("JOIN #aba4647");
        writer.WriteLine("CAP REQ :twitch.tv/commands twitch.tv/tags");
        writer.Flush();
        Debug.Log("Connect twitch chatting");
    }

    private void Update()
    {
        if(!twitchClient.Connected) return;
        ReadChat();
    }

    private void ReadChat()
    {
        if(twitchClient.Available > 0)
        {
            string message = reader.ReadLine();
            Debug.Log(message);
            if(message.Split(' ')[1] == "PING")
            {
                writer.WriteLine("PONG :tmi.twitch.tv");
                writer.Flush();
                return;
            }
            Chat chat = Parser.GetMessage(message);
            if(chat != null)
            {
                GameObject chat_ = ObjectPool.Get(
                    gameObject,
                    "Chat",
                    () => {
                        GameObject target = Instantiate(Chat, transform, false);
                        target.transform.SetAsLastSibling();
                        return target;
                    }
                );
                chat_.name = "Chat";
                chat_.GetComponent<ChatPanel>().RectTransform.anchoredPosition = new Vector2(0, -40);
                for(int i = 0; i < transform.childCount; i++)
                {
                    GameObject child = transform.GetChild(i).gameObject;
                    child.GetComponent<ChatPanel>().RectTransform.anchoredPosition += new Vector2(0, 80);
                }
                ChatPanel script = chat_.GetComponent<ChatPanel>();
                script.WriteContent(chat);
            }
        }
    }
}
