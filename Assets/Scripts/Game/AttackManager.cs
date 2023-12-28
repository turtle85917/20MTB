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
        if(target.name == "Jinhe") return;

#region 공격 대상/무기 사용자가 적일 경우, 적 데이터 가져오기
        EnemyPool enemyPool = null;
        EnemyPool sourceEnemyPool = null;
        if(target.CompareTag("Enemy")) enemyPool = EnemyManager.GetEnemy(target);
        if(source && source.CompareTag("Enemy")) sourceEnemyPool = EnemyManager.GetEnemy(source);
#endregion

        if(source != null && source.CompareTag("Enemy") && (sourceEnemyPool == null || sourceEnemyPool.weapon == null)) return;

        // 공격력과 무기 가져오기
        int power = Player.playerData.data.stats.Power;
        Weapon weapon = WeaponBundle.GetWeaponFromTarget(weaponId, Player.@object);
        if(sourceEnemyPool != null)
        {
            weapon = sourceEnemyPool.weapon;
            power = sourceEnemyPool.data.stats.Power;
        }

        // 치명타를 고려하여 들어갈 데미지 구하기
        bool critical = UnityEngine.Random.value < weapon.stats.CriticalHit;
        int damage = Mathf.CeilToInt(GetCalcDamage(power, critical ? weapon.stats.CriticalDamage : weapon.stats.Power, penetrate * weapon.stats.DecreasePower));

        // 공격하기
        if(target.CompareTag("Player"))
        {
            Weapon parasocialWeapon = Player.playerData.weapons.Find(item => item.weapon.weaponId == "パラソーシャル");
            if(parasocialWeapon != null && UnityEngine.Random.value < 0.4f && source != null)
            {
                sourceEnemyPool = EnemyManager.GetEnemy(source);
                // 치명타가 아닌 값으로 처리하기 위함
                damage = Mathf.CeilToInt(GetCalcDamage(sourceEnemyPool.data.stats.Power, sourceEnemyPool.weapon.stats.Power, penetrate * sourceEnemyPool.weapon.stats.DecreasePower) * 0.25f);
                if(damage > 0)
                {
                    sourceEnemyPool.health -= damage;
                    TextManager.WriteDamage(sourceEnemyPool.target, damage, false);
                }
            }
            else
            {
                Player.playerData.health -= damage;
                TextManager.WriteDamage(Player.@object, damage, critical);
            }
        }
        else
        {
            enemyPool.health -= damage;
            TextManager.WriteDamage(enemyPool.target, damage, critical);
        }
        if(postProcessFunc != null)
        {
            postProcessFunc(target.GetComponent<Affecter>());
        }
    }

    /// <summary>
    /// 특정 상수 값만큼 데미지를 입힙니다.
    /// </summary>
    /// <param name="value">데미지</param>
    /// <param name="target">공격할 대상</param>
    /// <param name="attackerPool">공격한 대상의 풀</param>
    public static void AttackTarget(int value, GameObject target, EnemyPool attackerPool)
    {
        if(target.CompareTag("Player"))
        {
            Weapon parasocialWeapon = Player.playerData.weapons.Find(item => item.weapon.weaponId == "パラソーシャル");
            if(parasocialWeapon != null && UnityEngine.Random.value < 0.4f)
            {
                attackerPool.health -= value * 0.25f;
                TextManager.WriteDamage(attackerPool.target, value, false);
            }
            else
            {
                Player.playerData.health -= value;
                TextManager.WriteDamage(Player.@object, value, false);
            }
        }
        else
        {
            EnemyPool enemyPool = EnemyManager.GetEnemy(target);
            if(enemyPool == null) return;
            enemyPool.health -= value;
            TextManager.WriteDamage(target, value, false);
        }
    }

    private static int GetCalcDamage(int characterPower, int weaponPower, int decreasePower)
    {
        return Mathf.CeilToInt(weaponPower + weaponPower / characterPower - decreasePower);
    }
}
