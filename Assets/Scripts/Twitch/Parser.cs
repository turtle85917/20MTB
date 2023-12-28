using System.Text.RegularExpressions;

public static class Parser
{
    public static Chat GetMessage(string message)
    {
        string[] chunk = message.Split(' ');
        switch(chunk[2])
        {
            case "PRIVMSG":
                Chat chat =  new Chat()
                {
                    message = ParseMessage(message)
                };
                ParseBadges(chat, message);
                return chat;
            default:
                return null;
        }
    }

    private static void ParseBadges(Chat chat, string value)
    {
        string[] chunk = value.Substring(1).Split(';');
        foreach(string item in chunk)
        {
            string key = item.Substring(0, item.IndexOf('='));
            string v = item.Substring(item.IndexOf('=') + 1);
            switch(key)
            {
                case "color":
                    chat.color = string.IsNullOrEmpty(v) ? "#ffffff" : v;
                    break;
                case "user-id":
                    chat.userId = v;
                    break;
                case "display-name":
                    chat.userName = v;
                    break;
            }
        }
    }

    private static string ParseMessage(string value)
    {
        return new Regex("PRIVMSG #.+? :(.+)").Match(value).Groups[1].Value;
    }
}
