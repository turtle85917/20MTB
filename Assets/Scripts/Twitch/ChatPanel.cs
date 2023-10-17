using System.Collections;
using TMPro;
using UnityEngine;

public class ChatPanel : MonoBehaviour
{
    public RectTransform RectTransform
    {
        get
        {
            return rectTransform;
        }
    }
    [SerializeField] private TMP_Text Nickname;
    [SerializeField] private TMP_Text Content;
    private RectTransform rectTransform;

    public void WriteContent(Chat chat)
    {
        Nickname.text = chat.username;
        Color color;
        ColorUtility.TryParseHtmlString(chat.color, out color);
        Nickname.color = color;
        Content.text = chat.message;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
