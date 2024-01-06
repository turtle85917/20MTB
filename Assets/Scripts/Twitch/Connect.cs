using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connect : MonoBehaviour
{
    [SerializeField] private string nick;
    [SerializeField] private string username;
    [SerializeField] private string oauth;
    [SerializeField] private GameObject Chat;
    private string channelName;
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    private float maxHeight;

    private readonly Dictionary<string, string[]> commands = new()
    {
        {"spawn", new string[]{"create", "생성"}},
        {"drops", new string[]{"드롭스"}},
        {"gift", new string[]{"보상", "선물"}},
        {"vote", new string[]{"투표", "보스", "boss"}}
    };
    private readonly Dictionary<string, string> giftTypes = new()
    {
        {"random", "랜덤"},
        {"boom", "꽝"},
        {"health", "체력"},
        {"enemy", "적"},
        {"weapon", "무기"}
    };
    private readonly Dictionary<Character, string> twitchChannelIds = new Dictionary<Character, string>()
    {
        {Character.Woowakgood, "woowakgood"},
        {Character.Ine, "vo_ine"},
        {Character.Jingburger, "jingburger"},
        {Character.Lilpa, "lilpaaaaaa"},
        {Character.Jururu, "cotton__123"},
        {Character.Gosegu, "gosegugosegu"},
        {Character.Viichan, "viichan6"}
    };

    public void ReconnectChannel(int character)
    {
        channelName = twitchChannelIds.GetValueOrDefault((Character)character);
        ConnectTwitchServer();
    }
    public void RemoveAllChats()
    {
        for(int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
    }
    private void ConnectTwitchServer()
    {
        if(twitchClient?.Connected == true)
        {
            twitchClient.Close();
            reader.Close();
            writer.Close();
            RemoveAllChats();
        }
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new(twitchClient.GetStream());
        writer = new(twitchClient.GetStream());
        Init();
    }

    private void Init()
    {
        writer.WriteLine("PASS " + oauth);
        writer.WriteLine("NICK " + nick);
        writer.WriteLine("USER " + username + " 8 * : " + username);
    #if UNITY_EDITOR
        writer.WriteLine("JOIN #pulto__");
    #else
        writer.WriteLine("JOIN #" + channelName);
    #endif
        writer.WriteLine("CAP REQ :twitch.tv/commands twitch.tv/tags");
        writer.Flush();
        Debug.Log("Connect twitch server");
        maxHeight = ((RectTransform)transform).sizeDelta.y;
    }

    private void Start()
    {
        if(GlobalSetting.instance.selectedChannel != -1)
        {
            ReconnectChannel(GlobalSetting.instance.selectedChannel);
        }
    }

    private void Update()
    {
        if(twitchClient == null) return;
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
                if(GlobalSetting.instance.showChatPanel)
                    CreateChatObject(chat);
                if(SceneManager.GetActiveScene().name == "Game")
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
            string key = commands.FirstOrDefault(item => item.Key == chunk[0] || item.Value.Contains(chunk[0])).Key;
            switch(key)
            {
                case "spawn":
                    if(chunk.Length < 1) return;
                    string weaponName = string.Join(' ', chunk[1..]);
                    GameObject enemy = Game.instance.usableWeaponsPanel.SpawnEnemy(chat, weaponName);
                    if(enemy != null)
                        TextManager.WriteTwitchNickname(enemy, chat);
                    break;
                case "drops":
                    Game.instance.drops.JoinDrops(chat);
                    break;
                case "gift":
                    if(Game.instance.drops.dropsActivator == null) return;
                    if(chat.userId == Game.instance.drops.dropsActivator.userId)
                    {
                        string giftType = chunk[1];
                        string giftKey = giftTypes.FirstOrDefault(item => item.Key == giftType || item.Value == giftType).Key;
                        if(giftKey == "random") giftKey = giftTypes.Keys.ToArray()[Random.Range(1, giftTypes.Keys.Count)];
                        if(giftKey == "weapon" && (string.IsNullOrEmpty(chunk[2]) || chunk.Length < 2)) return;
                        Game.instance.drops.SpawnDrop(giftKey, giftKey == "weapon" ? string.Join(' ', chunk[2..]) : null);
                    }
                    break;
                case "vote":
                    if(chunk.Length < 1) return;
                    int.TryParse(chunk[1], out int index);
                    if(index < 0 || index > 2) return;
                    Game.instance.bossVote.OnVote(chat, index - 1);
                    break;
            }
        }
    }
}
