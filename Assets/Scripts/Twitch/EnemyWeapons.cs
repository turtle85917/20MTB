using System.Collections.Generic;
using System.Linq;
using _20MTB.Utillity;
using UnityEngine;

public class EnemyWeapons : MonoBehaviour
{
    public List<Weapon> weapons;
    private WeaponItem[] weaponItems;
    private Dictionary<string, List<string>> spawners;
    private int userCount;

    public void PickupWeapons()
    {
        gameObject.SetActive(true);
        Weapon[] useableWeapons = WeaponBundle.GetWeapons(item => item.type == "N");
        List<Weapon> duplicatedWeapons = new List<Weapon>(3);
        foreach(WeaponItem weaponItem in weaponItems)
        {
            Weapon weapon;
            do weapon = useableWeapons[Random.Range(0, useableWeapons.Length)];
            while(duplicatedWeapons.Exists(item => item.weapon.weaponId == weapon.weapon.weaponId));
            weapons.Add(weapon);
            duplicatedWeapons.Add(weapon);
            spawners.Add(weapon.weapon.weaponId, new List<string>(){});
            weaponItem.UpdatePanel(weapon);
        }
    }

    public GameObject SpawnEnemy(string twitchUserId, string weaponId)
    {
        Weapon weapon = WeaponBundle.GetWeaponByName(weaponId);
        if(spawners.ContainsKey(weapon.weapon.weaponId))
        {
            spawners.TryGetValue(weapon.weapon.weaponId, out List<string> users);
            if(users.Contains(twitchUserId)) return null;
            users.Add(twitchUserId);
            userCount++;
            if(userCount > 50) return null;
            int index = spawners.Keys.ToList().IndexOf(weapon.weapon.weaponId);
            weaponItems[index].UpdateSlider(users.Count / userCount);
            GameObject enemy = EnemyManager.NewEnemy("Panzee");
            WeaponBundle.AddWeaponToTarget(enemy, weapon.weapon.weaponId);
            enemy.transform.position = GameUtils.MovePositionLimited(Player.@object.transform.position + (Vector3)Random.insideUnitCircle.normalized * 14);
            return enemy;
        }
        return null;
    }

    private void Awake()
    {
        weaponItems = GetComponentsInChildren<WeaponItem>();
        weapons = new List<Weapon>();
        spawners = new Dictionary<string, List<string>>();
    }

    private void Update()
    {
        if(userCount > 50)
        {
            gameObject.SetActive(false);
            weapons = new List<Weapon>();
            spawners = new Dictionary<string, List<string>>();
        }
    }
}
