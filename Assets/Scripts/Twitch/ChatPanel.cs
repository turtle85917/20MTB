using TMPro;
using UnityEngine;

public class ChatPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text Nickname;
    [SerializeField] private TMP_Text Content;

    public void WriteContent(Chat chat)
    {
        Nickname.text = chat.userName;
        ColorUtility.TryParseHtmlString(chat.color, out Color color);
        Nickname.color = color;
        Content.text = chat.message;
    }
}
