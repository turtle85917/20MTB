using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    private static TextManager instance;
    [SerializeField] private GameObject Text;

    public static void WriteDamage(GameObject target, int value, bool critical)
    {
        GameObject text = ObjectPool.Get(
            instance.gameObject,
            "Text",
            () => {
                GameObject obj = Instantiate(instance.Text, instance.transform, false);
                obj.name = "Text";
                return obj;
            }
        );
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)instance.transform, Camera.main.WorldToScreenPoint(targetPosition), Camera.main, out var localPoint);
        text.transform.localPosition = localPoint + new Vector2(0, 0.5f);
        Color color = Color.white;
        if(target.CompareTag("Player"))
        {
            color = Color.red;
        }
        else if(critical)
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
