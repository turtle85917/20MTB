using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    [SerializeField] private Image Logo;
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Desc;
    [SerializeField] private TMP_Text Level;
    [SerializeField] private GameObject LockedPanel;
    private Weapon weapon;

    public void UpdatePanel(Weapon currentWeapon)
    {
        weapon = currentWeapon;
        Logo.sprite = weapon.weapon.logo;
        Name.text = weapon.name;
        Level.text = "Level. " + (weapon.level + 1);
    }

    public void Locking(bool isLocked)
    {
        Desc.text = isLocked ? weapon.weapon.enemyDescription : weapon.weapon.playerDescription;
        LockedPanel.SetActive(isLocked && weapon.type != "N");
    }
}
