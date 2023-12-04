using System.Collections;
using System.Linq;
using UnityEngine;

public class KettleCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromPlayer("Kettle");
            yield return new WaitForSeconds(2f);
            Weapon selectWeapon = null;
            EnemyManager.EnemyPool selectEnemyPool = null;
            switch(weaponUser.tag)
            {
                case "Player":
                    Weapon[] filteredArray = Player.playerData.weapons.Where(item => item.type == "N" && item.weapon.weaponId != "Kettle").ToArray();
                    if(filteredArray.Length > 0)
                        selectWeapon = filteredArray[Random.Range(0, filteredArray.Length)];
                    break;
                case "Enemy":
                    EnemyManager.EnemyPool[] enemies = EnemyManager.GetEnemies().Where(item => item.weapon != null && item.weapon.weapon.weaponId != "Kettle").ToArray();
                    if(enemies.Length > 0)
                    {
                        selectEnemyPool = enemies[Random.Range(0, enemies.Length)];
                        selectWeapon = selectEnemyPool.weapon;
                    }
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
                script.weaponOwner = selectEnemyPool;
                script.Init();
                System.Type monoscript = selectWeapon.weapon.weaponCycleScriptFile.GetClass();
                BaseCycle baseCycle = System.Activator.CreateInstance(monoscript) as BaseCycle;
                script.StartCoroutine(baseCycle.Cycle(jinhe));
            }
        }
    }
}
