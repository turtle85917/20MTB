using System.Collections;
using TMPro;
using UnityEngine;

public class TalkBalloon : MonoBehaviour
{
    [SerializeField] private GameObject BalloonBody;
    [SerializeField] private TMP_Text BalloonContent;
    [SerializeField] private string[] story;
    private string content;
    private int index;

    private const float TYPING_DELAY = 0.1f;
    private const float PADDING_LEFT = 50;
    private const int BALLOON_CONTENT_DEFAULT_X = 244;
    private const float BALLOON_BODY_DEFAULT_Y = 82.45f;
    private const float BODY_OFFSET = 1.225f;

    public void Next()
    {
        StartTyping(story[index++]);
    }

    private void StartTyping(string content_)
    {
        content = content_;
        BalloonContent.text = string.Empty;
        StartCoroutine(TypingContent());
    }

    private IEnumerator TypingContent()
    {
        for(int i = 0; i < content.Length; i++)
        {
            BalloonContent.text += content[i];
            FlexBalloon();
            yield return new WaitForSeconds(TYPING_DELAY);
        }
    }

    private void FlexBalloon()
    {
        RectTransform rect = (RectTransform)BalloonContent.transform;
        RectTransform bodyRect = (RectTransform)BalloonBody.transform;
        float x = BalloonContent.preferredWidth / BALLOON_CONTENT_DEFAULT_X;
        bodyRect.sizeDelta = new Vector2(BalloonContent.preferredWidth + PADDING_LEFT, BalloonContent.bounds.size.y + PADDING_LEFT);
        bodyRect.anchoredPosition = new Vector2(0, (bodyRect.sizeDelta.y - BALLOON_BODY_DEFAULT_Y) / 2 + BODY_OFFSET);
        rect.sizeDelta = new Vector2(BalloonContent.preferredWidth, BalloonContent.bounds.size.y);
        rect.anchoredPosition = new Vector2(0, bodyRect.anchoredPosition.y + 5);
    }
}
