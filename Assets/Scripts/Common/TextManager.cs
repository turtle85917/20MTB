using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    private static TextManager instance;
    [SerializeField] private GameObject TwitchNickname;
    [SerializeField] private GameObject DamageText;
    [SerializeField] private GameObject ComboText;

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
        string content = value.ToString();
        if(target.CompareTag("Player")) content = '-' + value.ToString();
        if(value < 0) content = '+' + (-value).ToString();
        if (target.CompareTag("Player"))
            color = Color.red;
        else if (critical)
            color = Color.yellow;
        else
            color = Color.white;
        if(value < 0)
            color = Color.green;
        TMP_Text tmpText = text.GetComponent<TMP_Text>();
        tmpText.text = content;
        tmpText.color = color;
    }

    public static void WriteComboText(GameObject target, int combo)
    {
        GameObject text = ObjectPool.Get(instance.gameObject, "ComboText", instance.ComboText);
        text.transform.localPosition = (Vector2)target.transform.position + Vector2.up * 0.15f;
        TMP_Text tmpText = text.GetComponent<TMP_Text>();
        tmpText.text = combo.ToString().PadLeft(4, '0');
    }

    private void Awake()
    {
        instance = this;
    }
}
