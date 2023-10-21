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
    private List<Chat> chatQueue;
    private readonly Dictionary<string, string[]> commands = new(){
        {"spawn", new string[]{"spawn", "create", "생성"}}
    };

    private void Awake()
    {
        twitchClient = new("irc.chat.twitch.tv", 6667);
        reader = new(twitchClient.GetStream());
        writer = new(twitchClient.GetStream());
        chatQueue = new(){};
    }

    private void Start()
    {
        writer.WriteLine("PASS " + oauth);
        writer.WriteLine("NICK " + nick);
        writer.WriteLine("USER " + username + " 8 * : " + username);
        writer.WriteLine("JOIN #pulto__");
        writer.WriteLine("CAP REQ :twitch.tv/commands twitch.tv/tags");
        writer.Flush();
        Debug.Log("Connect twitch chatting");
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
            if(chatQueue.Count > 4)
            {
                Chat oldChat = chatQueue.First();
                oldChat.target.SetActive(false);
                chatQueue.Remove(oldChat);
            }
            if(chat != null)
            {
                CreateNewChatObject(chat);
                if(chat.message.StartsWith("!"))
                {
                    string[] chunk = chat.message.Substring(1).Split(' ');
                    string key = commands.FirstOrDefault(item => item.Value.Contains(chunk[0])).Key;
                    switch(key)
                    {
                        case "spawn":
                            string weaponName = chunk[1];
                            Weapon weapon = WeaponBundle.GetWeaponByName(weaponName);
                            if(weapon != null)
                            {
                                GameObject enemy = EnemyManager.NewEnemy("Panzee");
                                EnemyManager.AddWeaponToEnemy(enemy, weapon.weapon.WeaponId);
                                enemy.transform.position = Game.MovePositionLimited(Player.instance.transform.position + (Vector3)Random.insideUnitCircle.normalized * 15, 0);
                            }
                            break;
                    }
                }
            }
        }
    }

    private void CreateNewChatObject(Chat chat)
    {
        GameObject chat_ = ObjectPool.Get(
            gameObject,
            "Chat",
            () => Instantiate(Chat, transform, false)
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
        chat.target = chat_;
        chatQueue.Add(chat);
    }
}
