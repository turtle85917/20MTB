using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image logo;
    [SerializeField] private TMP_Text Name;

    public void UpdatePanel(Weapon weapon)
    {
        slider.value = 0;
        logo.sprite = weapon.weapon.logo;
        Name.text = weapon.name;
    }

    public void UpdateSlider(float ratio)
    {
        slider.value = ratio;
    }
}
