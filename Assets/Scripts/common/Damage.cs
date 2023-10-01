using TMPro;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public static Damage instance;
    [SerializeField] private GameObject Text;

    public void WriteDamage(GameObject target, int value)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if(!child.activeSelf)
            {
                child.SetActive(true);
                ChangeTextObject(target, value, child);
                return;
            }
        }
        CreateTextObject(target, value);
    }

    private void Awake()
    {
        instance = this;
    }

    private void ChangeTextObject(GameObject target, int value, GameObject TextObject)
    {
        TextObject.transform.localPosition = target.transform.position + Vector3.back * 20;
        TMP_Text tmpText = TextObject.GetComponent<TMP_Text>();
        tmpText.text = (target.CompareTag("Player") ? '-' : string.Empty) + value.ToString();
        tmpText.color = target.CompareTag("Player")
            ? Color.red
            : Color.white
        ;
    }

    private void CreateTextObject(GameObject target, int value)
    {
        GameObject text = Instantiate(Text, transform, false);
        text.name = "Text" + transform.childCount;
        text.transform.localPosition = target.transform.position + Vector3.back * 20;
        TMP_Text tmpText = text.GetComponent<TMP_Text>();
        tmpText.text = (target.CompareTag("Player") ? '-' : string.Empty) + value.ToString();
        tmpText.color = target.CompareTag("Player")
            ? Color.red
            : Color.white
        ;
    }
}
