using TMPro;
using UnityEngine;

public class SelectCharacterManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text WeaponName;
    [SerializeField] private TMP_Text WeaponDesc;
    [SerializeField] private GameObject arrowDown;
    [SerializeField] private RuntimeAnimatorController[] animators;
    [SerializeField] private WeaponData[] defaultWeapons;
    private readonly string[] defaultWeaponNames = new string[]{
        "왁초리",
        "마법봉",
        "무에타이",
        "비명",
        "도장 쿵",
        "십자 머리핀",
        "다이아 검"
    };

    private Vector2 arrowDownPosition;

    public void OnEnterSlotCard(int index, GameObject slot)
    {
        animator.runtimeAnimatorController = animators[index];
        arrowDownPosition = new Vector2(-800 / 2 + ((RectTransform)slot.transform).anchoredPosition.x, -130);
        WeaponName.text = defaultWeaponNames[index];
        WeaponDesc.text = defaultWeapons[index].playerDescription;
    }

    private void Start()
    {
        Camera.main.transparencySortAxis = new Vector3(0, 0, 1);
        animator.runtimeAnimatorController = animators[0];
        WeaponName.text = defaultWeaponNames[0];
        WeaponDesc.text = defaultWeapons[0].playerDescription;
    }

    private void Update()
    {
        if(arrowDownPosition != Vector2.zero)
            ((RectTransform)arrowDown.transform).anchoredPosition = Vector2.Lerp(((RectTransform)arrowDown.transform).anchoredPosition, arrowDownPosition, 0.2f);
    }
}
