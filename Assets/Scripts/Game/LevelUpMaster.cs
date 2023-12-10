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

    private void SelectWeapon(Weapon weapon)
    {
        if(Player.playerData.weapons.Exists(item => item.weapon.weaponId == weapon.weapon.weaponId))
        {
            WeaponBundle.UpgradeTargetsWeapon(Player.@object, weapon.weapon.weaponId);
        }
        else
        {
            WeaponBundle.AddWeaponToTarget(Player.@object, weapon.weapon.weaponId);
        }
        Game.Resume();
        LevelUpPanel.SetActive(false);
    }

    private void ChangeWeaponPanels()
    {
        List<Weapon> weapons = new List<Weapon>();
        List<Weapon> leftWeapons = WeaponBundle.GetWeapons(item => !Player.playerData.weapons.Exists(w => w.weapon.weaponId == item.weapon.weaponId) && !weapons.Exists(w => w.weapon.weaponId == item.weapon.weaponId) && item.type != "D").ToList();
        AddWeapon(0.05f);
        AddWeapon(0.2f);
        AddWeapon(0.35f);
        for(int i = 0; i < weapons.Count; i++)
        {
            weaponPanels[i].UpdatePanel(weapons[i]);
            int index = i;
            weaponPanels[i].GetComponent<Button>().onClick.RemoveAllListeners();
            weaponPanels[i].GetComponent<Button>().onClick.AddListener(() => SelectWeapon(weapons[index]));
        }
        ChangeCategory(Category.Player);

        void AddWeapon(float random)
        {
            GetDecideWeapon(random, out Weapon decideWeapon);
            weapons.Add(decideWeapon);
            leftWeapons.Remove(decideWeapon);
        }

        void GetDecideWeapon(float random, out Weapon decideWeapon)
        {
            Weapon[] usableWeapons = Player.playerData.weapons
                .FindAll(item => !weapons.Exists(w => w.weapon.weaponId == item.weapon.weaponId))
                .Where(item => item.weapon.levels.Length > item.level)
                .ToArray()
            ;
            if(usableWeapons.Length == 0) decideWeapon = leftWeapons[Random.Range(0, leftWeapons.Count)];
            else if(usableWeapons.Length == 6 || Random.value > random) decideWeapon = usableWeapons[Random.Range(0, usableWeapons.Length)];
            else decideWeapon = leftWeapons[Random.Range(0, leftWeapons.Count)];
        }
    }
}
