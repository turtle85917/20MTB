public static class Parser
{
    public static Chat GetMessage(string message)
    {
        string[] chunk = message.Split(' ');
        switch(chunk[1])
        {
            case "PRIVMSG":
                string nickname = ParseNickname(chunk[0]);
                string channel = chunk[2].Substring(1);
                string messageContent = chunk[3].Substring(1);
                return new Chat(){
                    nickname = nickname,
                    channel = channel,
                    message = messageContent
                };
            default:
                return null;
        }
    }

    private static string ParseNickname(string value)
    {
        return value.Substring(1, value.IndexOf('!') - 1);
    }
}
