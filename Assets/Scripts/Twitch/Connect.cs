using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private float maxHeight;
    private readonly Dictionary<string, string[]> commands = new()
    {
        {"spawn", new string[]{"spawn", "create", "생성"}},
        {"drops", new string[]{"드롭스", "drops"}}
    };

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
        writer.WriteLine("JOIN #viichan6");
        writer.WriteLine("CAP REQ :twitch.tv/commands twitch.tv/tags");
        writer.Flush();
        Debug.Log("Connect twitch chatting");
        maxHeight = ((RectTransform)transform).sizeDelta.y;
    }

    private void Update()
    {
        if(!twitchClient.Connected)
        {
            Debug.Log("Disconnect");
            return;
        }
        ReadChat();
    }

    private void ReadChat()
    {
        if(twitchClient.Available > 0)
        {
            string message = reader.ReadLine();
            Debug.Log(message);
            if(message.StartsWith("PING"))
            {
                writer.WriteLine("PONG :tmi.twitch.tv");
                writer.Flush();
                return;
            }
            Chat chat = Parser.GetMessage(message);
            if(chat != null)
            {
                CreateChatObject(chat);
                RunCommand(chat);
            }
        }
    }

    private void CreateChatObject(Chat chat)
    {
        GameObject chatObject = ObjectPool.Get(gameObject, "Chat", Chat);
        ((RectTransform)chatObject.transform).anchoredPosition = new Vector2(0, -40);
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            RectTransform rectTransform = (RectTransform)child.transform;
            rectTransform.anchoredPosition += new Vector2(0, 80);
            if(rectTransform.anchoredPosition.y > maxHeight + 80)
            {
                child.SetActive(false);
                child.transform.SetAsLastSibling();
            }
        }
        ChatPanel script = chatObject.GetComponent<ChatPanel>();
        script.WriteContent(chat);
    }

    private void RunCommand(Chat chat)
    {
        if(chat.message.StartsWith("!"))
        {
            string[] chunk = chat.message.Substring(1).Split(' ');
            string key = commands.FirstOrDefault(item => item.Value.Contains(chunk[0])).Key;
            switch(key)
            {
                case "spawn":
                    string weaponName = string.Join(' ', chunk[1..]);
                    GameObject enemy = Game.instance.usableWeaponsPanel.SpawnEnemy(chat, weaponName);
                    if(enemy != null)
                        TextManager.WriteTwitchNickname(enemy, chat);
                    break;
                case "drops":
                    break;
            }
        }
    }
}
