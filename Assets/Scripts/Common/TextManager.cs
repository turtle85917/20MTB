using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    private static TextManager instance;
    [SerializeField] private GameObject TwitchNickname;
    [SerializeField] private GameObject DamageText;

    public static void WriteTwitchNickname(GameObject target, Chat chat)
    {
        GameObject text = ObjectPool.Get(instance.gameObject, "TwitchNickname", instance.TwitchNickname);
        text.transform.localPosition = (Vector2)target.transform.position + Vector2.down * 1.3f;
        TMP_Text tmpText = text.GetComponent<TMP_Text>();
        tmpText.text = chat.username;
        ColorUtility.TryParseHtmlString(chat.color, out Color color);
        tmpText.color = color;
        target.GetComponent<Enemy>().text = text;
    }

    public static void WriteDamage(GameObject target, int value, bool critical)
    {
        GameObject text = ObjectPool.Get(instance.gameObject, "DamageText", instance.DamageText);
        text.transform.localPosition = (Vector2)target.transform.position + Vector2.up * 0.5f;
        Color color;
        if (target.CompareTag("Player"))
        {
            color = Color.red;
        }
        else if (critical)
        {
            color = Color.yellow;
        }
        else
        {
            color = Color.white;
        }
        TMP_Text tmpText = text.GetComponent<TMP_Text>();
        tmpText.text = (target.CompareTag("Player") ? '-' : string.Empty) + value.ToString();
        tmpText.color = color;
    }

    private void Awake()
    {
        instance = this;
    }
}
