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
        ColorUtility.TryParseHtmlString(chat.color, out Color color);
        Nickname.color = color;
        Content.text = chat.message;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
