public static class Parser
{
    public static Chat GetMessage(string message)
    {
        string[] chunk = message.Split(' ');
        switch(chunk[2])
        {
            case "PRIVMSG":
                string channel = ParseChannel(chunk[3]);
                Chat chat =  new Chat()
                {
                    channel = channel,
                    message = ParseMessage(message),
                    target = null
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
                    chat.color = v;
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

    private static string ParseChannel(string value)
    {
        return value.Substring(1);
    }

    private static string ParseMessage(string value)
    {
        return value.Split(':')[^1];
    }
}
