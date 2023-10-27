using System;
using UnityEngine;

public static class AttackManager
{
    public static void AttackTarget(string weaponId, GameObject target, int penetrate, Action<Affecter> postProcessFunc = null)
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
        TextManager.WriteDamage(target, damage, false);
        if(target.CompareTag("Player"))
        {
            Player.playerData.health -= damage;
        }
        else
        {
            enemyPool.health -= damage;
        }
        if(postProcessFunc != null)
        {
            postProcessFunc(target.GetComponent<Affecter>());
        }
    }
}
