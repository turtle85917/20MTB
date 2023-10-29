using System;
using UnityEngine;

public static class AttackManager
{
    /// <summary>
    /// target의 대미지를 입힙니다.
    /// </summary>
    /// <param name="weaponId">무기 ID</param>
    /// <param name="target">공격 대상</param>
    /// <param name="penetrate">관통 수</param>
    /// <param name="postProcessFunc">후처리 함수 (lambda)</param>
    /// <param name="source">무기 사용자</param>
    public static void AttackTarget(string weaponId, GameObject target, int penetrate, Action<Affecter> postProcessFunc = null, GameObject source = null)
    {
        Weapon weapon = WeaponBundle.GetWeapon(weaponId);
        EnemyManager.EnemyPool enemyPool = null;
        int power = Player.playerData.data.stats.Power;
        if(target.CompareTag("Enemy"))
        {
            enemyPool = EnemyManager.GetEnemy(target);
            power = enemyPool.data.stats.Power;
        }
        int damage = weapon.stats.Power + weapon.stats.Power / power - penetrate * weapon.stats.DecreasePower;
        if(target.CompareTag("Player"))
        {
            Weapon parasocialWeapon = Player.playerData.weapons.Find(item => item.weapon.weaponId == "パラソーシャル");
            if(parasocialWeapon != null && UnityEngine.Random.value < 0.4f && source != null)
            {
                enemyPool = EnemyManager.GetEnemy(source);
                damage = Mathf.RoundToInt(damage * 0.25f);
                if(damage > 0)
                {
                    enemyPool.health -= damage;
                    TextManager.WriteDamage(enemyPool.target, damage, false);
                }
            }
            else
            {
                Player.playerData.health -= damage;
                TextManager.WriteDamage(Player.@object, damage, false);
            }
        }
        else
        {
            enemyPool.health -= damage;
            TextManager.WriteDamage(target, damage, false);
        }
        if(postProcessFunc != null)
        {
            postProcessFunc(target.GetComponent<Affecter>());
        }
    }
}
