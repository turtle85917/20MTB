using System;
using _20MTB.Stats;
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
    /// <param name="plusForce">추가적인 힘 (곱적용으로 적용함.)</param>
    public static void AttackTarget(string weaponId, GameObject target, int penetrate, Action<Affecter> postProcessFunc = null, GameObject source = null, float plusForce = 0)
    {
        if(target.name == "Jinhe") return;

        Weapon weapon = WeaponBundle.GetWeapon(weaponId);

#region 공격 대상/무기 사용자가 적일 경우, 적 데이터 가져오기
        EnemyManager.EnemyPool enemyPool = null;
        EnemyManager.EnemyPool sourceEnemyPool = null;
        if(target.CompareTag("Enemy")) enemyPool = EnemyManager.GetEnemy(target);
        if(source && source.CompareTag("Enemy")) sourceEnemyPool = EnemyManager.GetEnemy(source);
#endregion

        if(source != null && source.CompareTag("Enemy") && (sourceEnemyPool == null || sourceEnemyPool.weapon == null)) return;

        bool critical = UnityEngine.Random.value < (source?.CompareTag("Enemy") ?? false ? sourceEnemyPool.weapon.stats.CriticalHit : weapon.stats.CriticalHit);
        int damage = Mathf.CeilToInt(GetCalcDamage(Player.playerData.data.stats.Power, GetWeaponPower(critical, weapon.stats), penetrate * weapon.stats.DecreasePower) * (plusForce + 1));
        if(sourceEnemyPool != null) damage = GetCalcDamage(sourceEnemyPool.data.stats.Power, GetWeaponPower(critical, sourceEnemyPool.weapon.stats), penetrate * sourceEnemyPool.weapon.stats.DecreasePower);
        if(target.CompareTag("Player"))
        {
            Weapon parasocialWeapon = Player.playerData.weapons.Find(item => item.weapon.weaponId == "パラソーシャル");
            if(parasocialWeapon != null && UnityEngine.Random.value < 0.4f && source != null)
            {
                sourceEnemyPool = EnemyManager.GetEnemy(source);
                // 크리티컬 데미지가 아닌 값으로 처리하기 위함
                damage = Mathf.CeilToInt(Mathf.RoundToInt(GetCalcDamage(sourceEnemyPool.data.stats.Power, sourceEnemyPool.weapon.stats.Power, penetrate * sourceEnemyPool.weapon.stats.DecreasePower) * 0.25f) * (plusForce + 1));
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

    public static void HealTarget(int value, GameObject target)
    {
        if(target.CompareTag("Player"))
        {
            Player.playerData.health += value;
            TextManager.WriteDamage(Player.@object, -value, false);
        }
        else
        {
            EnemyManager.EnemyPool enemyPool = EnemyManager.GetEnemy(target);
            enemyPool.health += value;
            TextManager.WriteDamage(enemyPool.target, -value, false);
        }
    }

    private static int GetWeaponPower(bool critical, WeaponStats stats)
    {
        return critical ? stats.CriticalDamage : stats.Power;
    }

    /// <summary>
    /// 계산된 데미지를 반환합니다.
    /// </summary>
    /// <param name="characterPower">공격하는 대상의 힘</param>
    /// <param name="weaponPower">무기의 공격력</param>
    /// <param name="decreasePower">최종 값에서 깎이는 데미지</param>
    private static int GetCalcDamage(int characterPower, int weaponPower, int decreasePower)
    {
        return Mathf.CeilToInt(weaponPower + weaponPower / characterPower - decreasePower);
    }
}
