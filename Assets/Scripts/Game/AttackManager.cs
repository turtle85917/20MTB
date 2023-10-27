using System;
using UnityEngine;

public static class AttackManager
{
    public static void AttackTarget(string weaponId, GameObject target, int penetrate, Action<Affecter> postProcessFunc = null)
    {
        Weapon weapon = WeaponBundle.GetWeapon(weaponId);
        int damage = weapon.stats.Power - penetrate * weapon.stats.DecreasePower;
        TextManager.WriteDamage(target, damage, false);
        if(target.CompareTag("Player"))
        {
            Player.playerData.health -= damage;
        }
        else
        {
            EnemyManager.EnemyPool enemyPool = EnemyManager.GetEnemy(target);
            enemyPool.health -= damage;
        }
        if(postProcessFunc != null)
        {
            postProcessFunc(target.GetComponent<Affecter>());
        }
    }
}
