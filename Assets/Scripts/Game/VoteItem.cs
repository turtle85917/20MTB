using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoteItem : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image logo;
    [SerializeField] private TMP_Text Name;

    public void UpdateWeaponPanel(Weapon weapon)
    {
        slider.value = 0;
        logo.sprite = weapon.weapon.logo;
        Name.text = weapon.name;
    }

    public void UpdatePanel(Sprite sprite, string name)
    {
        slider.value = 0;
        logo.sprite = sprite;
        Name.text = name;
    }

    public void UpdateSlider(float ratio)
    {
        slider.value = ratio;
    }
}
