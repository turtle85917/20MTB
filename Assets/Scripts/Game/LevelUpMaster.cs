using System.Collections.Generic;
using System.Linq;
using _20MTB.Utillity;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMaster : MonoBehaviour
{
    [SerializeField] private GameObject LevelUpPanel;
    [SerializeField] private Button[] categoryButtons;
    [SerializeField] private WeaponPanel[] weaponPanels;

    private enum Category
    {
        Player,
        Enemy
    }

    private void Start()
    {
        for(int i = 0; i < categoryButtons.Length; i++)
        {
            int index = i;
            categoryButtons[i].onClick.AddListener(() => ChangeCategory((Category)index));
        }
        LevelUpPanel.SetActive(false);
    }

    private void Update()
    {
        if(Player.playerData.exp >= GameUtils.GetNeedExpFromLevel())
        {
            Game.Pause();
            LevelUpPanel.SetActive(true);
            ChangeWeaponPanels();
            Player.playerData.exp -= GameUtils.GetNeedExpFromLevel();
            Player.playerData.level++;
        }
    }

    private void ChangeCategory(Category newCategory)
    {
        foreach(Button button in categoryButtons) button.interactable = true;
        categoryButtons[(int)newCategory].interactable = false;
        foreach(WeaponPanel weaponPanel in weaponPanels) weaponPanel.Locking(newCategory == Category.Enemy);
    }

    private void ChangeWeaponPanels()
    {
        List<Weapon> weapons = new List<Weapon>();
        List<Weapon> leftWeapons = WeaponBundle.GetWeapons(item => !Player.playerData.weapons.Exists(w => w.weapon.weaponId == item.weapon.weaponId) && !weapons.Exists(w => w.weapon.weaponId == item.weapon.weaponId) && item.type != "D").ToList();
        List<Weapon> playerWeapons = Player.playerData.weapons.FindAll(item => !weapons.Exists(w => w.weapon.weaponId == item.weapon.weaponId)).ToList();
        AddWeapon(0.01f);
        AddWeapon(0.1f);
        AddWeapon(0.25f);
        for(int i = 0; i < weapons.Count; i++) weaponPanels[i].UpdatePanel(weapons[i]);
        ChangeCategory(Category.Player);

        void AddWeapon(float random)
        {
            Weapon decideWeapon;
            if(Random.value <= random) decideWeapon = leftWeapons[Random.Range(0, leftWeapons.Count)];
            else decideWeapon = playerWeapons[Random.Range(0, playerWeapons.Count)];
            weapons.Add(decideWeapon);
            leftWeapons.Remove(decideWeapon);
            playerWeapons.Remove(decideWeapon);
        }
    }
}
