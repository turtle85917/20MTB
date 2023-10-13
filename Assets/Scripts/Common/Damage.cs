using TMPro;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public static Damage instance {get; private set;}
    [SerializeField] private GameObject Text;

    public void WriteDamage(GameObject target, int value, bool critical)
    {
        GameObject text = ObjectPool.Get(
            gameObject,
            "Text",
            () => Instantiate(Text, transform, false)
        );
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, Camera.main.WorldToScreenPoint(targetPosition), Camera.main, out var localPoint);
        text.name = "Text";
        text.transform.localPosition = localPoint + Vector2.up * 20;
        TMP_Text tmpText = text.GetComponent<TMP_Text>();
        tmpText.text = (target.CompareTag("Player") ? '-' : string.Empty) + value.ToString();
        tmpText.color = target.CompareTag("Player")
            ? Color.red
            : critical
            ? Color.yellow
            : Color.white
        ;
    }

    private void Awake()
    {
        instance = this;
    }
}
