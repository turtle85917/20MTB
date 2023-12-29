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
        MatchCollection chunk = new Regex("\\b(.+?)=(.*?);\\b").Matches(value);
        for(int i = 0; i < chunk.Count; i++)
        {
            string k = chunk[i].Groups[1].Value;
            string v = chunk[i].Groups[2].Value;
            switch(k)
            {
                case "color":
                    chat.color = v == string.Empty ? "#ffffff" : v;
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
