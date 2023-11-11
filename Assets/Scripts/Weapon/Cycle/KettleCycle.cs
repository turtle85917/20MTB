using System.Collections;
using System.Linq;
using UnityEngine;

public class KettleCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Kettle");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            Weapon selectWeapon = null;
            switch(weaponUser.tag)
            {
                case "Player":
                    Weapon[] filteredArray = Player.playerData.weapons.Where(item => item.type == "N" && item.weapon.weaponId != "Kettle").ToArray();
                    if(filteredArray.Length > 0)
                        selectWeapon = filteredArray[Random.Range(0, filteredArray.Length)];
                    break;
                case "Enemy":
                    EnemyManager.EnemyPool[] enemies = EnemyManager.GetEnemies();
                    if(enemies.Length > 0)
                        selectWeapon = enemies[Random.Range(0, enemies.Length)].weapon;
                    break;
            }
            if(selectWeapon != null)
            {
                GameObject jinhe = ObjectPool.Get(Game.PoolManager, "Jinhe", (GameObject)weapon.weapon.resources[0]);
                jinhe.tag = weaponUser.tag; // 선과 악이 없음
                jinhe.transform.position = weaponUser.transform.position + Vector3.right * 3;
                Jinhe script = jinhe.GetComponent<Jinhe>();
                script.life =
                    weaponUser.CompareTag("Enemy")
                    ? EnemyManager.GetEnemy(weaponUser).weapon.stats.Life
                    : weapon.stats.Life
                ;
                script.weaponUser = weaponUser;
                script.Init();
                System.Type monoscript = selectWeapon.weapon.weaponCycleScriptFile.GetClass();
                BaseCycle baseCycle = System.Activator.CreateInstance(monoscript) as BaseCycle;
                script.StartCoroutine(baseCycle.Cycle(jinhe));
            }
        }
    }
}
