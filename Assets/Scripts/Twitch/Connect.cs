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
        writer.WriteLine("JOIN #boringmogi");
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
            if(message.Split(' ')[1] == "PING")
            {
                writer.WriteLine("PONG :tmi.twitch.tv");
                writer.Flush();
            }
            Chat chat = Parser.GetMessage(message);
            // TODO: ...
        }
    }
}
