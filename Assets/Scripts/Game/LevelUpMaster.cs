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
            weaponPanels[i].GetComponent<Button>().onClick.RemoveAllListeners();
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
        Weapon foundWeapon = Player.playerData.weapons.Find(item => item.weapon.weaponId == weapon.weapon.weaponId);
        if(foundWeapon != null)
        {
            WeaopnIncreaseStat weaopnIncreaseStat = weapon.weapon.levels[foundWeapon.level++];
            foreach(var field in weaopnIncreaseStat.GetType().GetFields())
            {
                // NOTE: 발사체는 곱적용
                var weaponField = foundWeapon.stats.GetType().GetField(field.Name);
                if(field.FieldType.Name == "Single")
                {
                    weaponField.SetValue(foundWeapon.stats, (float)weaponField.GetValue(foundWeapon.stats) + (float)field.GetValue(weaopnIncreaseStat));
                }
                else
                {
                    weaponField.SetValue(foundWeapon.stats, (int)weaponField.GetValue(foundWeapon.stats) + (int)field.GetValue(weaopnIncreaseStat));
                }
            }
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
        List<Weapon> playerWeapons = Player.playerData.weapons.FindAll(item => !weapons.Exists(w => w.weapon.weaponId == item.weapon.weaponId)).ToList();
        AddWeapon(0.01f);
        AddWeapon(0.1f);
        AddWeapon(0.25f);
        for(int i = 0; i < weapons.Count; i++)
        {
            weaponPanels[i].UpdatePanel(weapons[i]);
            int index = i;
            weaponPanels[i].GetComponent<Button>().onClick.AddListener(() => SelectWeapon(weapons[index]));
        }
        ChangeCategory(Category.Player);

        void AddWeapon(float random)
        {
            GetDecideWeapon(random, out Weapon decideWeapon);
            weapons.Add(decideWeapon);
            leftWeapons.Remove(decideWeapon);
            playerWeapons.Remove(decideWeapon);
        }

        void GetDecideWeapon(float random, out Weapon decideWeapon)
        {
            if(Random.value <= random || playerWeapons.Count == 0) decideWeapon = leftWeapons[Random.Range(0, leftWeapons.Count)];
            else
            {
                Weapon weapon = playerWeapons[Random.Range(0, playerWeapons.Count)];
                // NOTE: 만렙일 경우, 재검색
                if(weapon.weapon.levels.Length == weapon.level) GetDecideWeapon(random, out Weapon w);
                decideWeapon = weapon;
            }
        }
    }
}
