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
    private Animator animator;
    private RectTransform rectTransform;

    public void WriteContent(Chat chat)
    {
        Nickname.text = chat.username;
        Color color;
        ColorUtility.TryParseHtmlString(chat.color, out color);
        Nickname.color = color;
        Content.text = chat.message;
    }

    public void HideAnimEnd()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetTrigger("fadeOut");
    }
}
