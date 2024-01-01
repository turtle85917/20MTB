using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private SelectCharacterManager selectCharacterManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        GlobalSetting.instance.playingCharacter = (Character)transform.GetSiblingIndex();
        SceneManager.LoadScene("Game");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectCharacterManager.OnEnterSlotCard(transform.GetSiblingIndex(), gameObject);
    }

    private void Start()
    {
        selectCharacterManager = Camera.main.GetComponent<SelectCharacterManager>();
    }
}
