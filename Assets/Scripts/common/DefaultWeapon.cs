using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultWeapon : MonoBehaviour
{
    [SerializeField] private GameObject Blow;
    [SerializeField] private GameObject Star;
    [SerializeField] private GameObject Ring;
    [SerializeField] private GameObject MagicCircle;
    [SerializeField] private GameObject HeadpinPrefab;
    private Weapon weapon;

    private void Start()
    {
        weapon = WeaponBundle.GetWeapon(Game.instance.playerData.defaultWeapon);
        switch(weapon.weapon.WeaponId)
        {
            case "Wakchori":
                StartCoroutine(Wakchori());
                break;
            case "MagicWand":
                StartCoroutine(MagicWand());
                break;
            case "MuayThai":
                StartCoroutine(MuayThai());
                break;
            case "Lilpaaaaaa":
                StartCoroutine(Lilpaaaaaa());
                break;
            case "StampPlump":
                StartCoroutine(StampPlump());
                break;
            case "Headpin":
                StartCoroutine(Headpin());
                break;
        }
    }

    private IEnumerator Wakchori()
    {
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 distance = Player.instance.transform.position - (Vector3)mousePosition;
            GameObject blow = ObjectPool.Get(
                Game.instance.PoolManager,
                "Blow",
                () => Instantiate(Blow, Game.instance.PoolManager.transform, false)
            );
            blow.name = "Blow";
            blow.transform.rotation = LookAtTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Blow script = blow.GetComponent<Blow>();
            script.Reset(weapon.stats, distance.normalized * -1);
            yield return new WaitForSeconds(weapon.stats.Life);
            blow.SetActive(false);
        }
    }

    private IEnumerator MagicWand()
    {
        List<GameObject> targets = new(){};
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            for(int i = 0; i < weapon.stats.ProjectileCount; i++)
            {
                GameObject star = ObjectPool.Get(
                    Game.instance.PoolManager,
                    "Star",
                    () => Instantiate(Star, Game.instance.PoolManager.transform, false)
                );
                star.name = "Star";
                star.transform.localPosition = Player.instance.transform.position;
                star.GetComponent<Star>().Reset(weapon.stats, targets);
            }
        }
    }

    private IEnumerator MuayThai()
    {
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            List<GameObject> enemy = Scanner.ScanAll(Player.instance.transform.position, 10, "Enemy");
            Player.instance.Attack();
            Vector2 direction = Player.instance.lastMovement;
            if(enemy.Count > 0)
            {
                enemy = enemy.OrderBy(item => Vector3.Distance(item.transform.position, Player.instance.transform.position)).ToList();
                for(int i = 0; i < weapon.stats.Through; i++)
                {
                    if(enemy.Count <= i) break;
                    EnemyPool enemyPool = EnemyManager.instance.GetEnemy(enemy[i]);
                    int deal = Game.instance.GetDamage(weapon.stats.Power) - i * weapon.stats.DecreasePower;
                    enemyPool.health -= deal;
                    Damage.instance.WriteDamage(enemyPool.target, deal);
                }
            }
            else
            {
                GameObject blow = ObjectPool.Get(
                    Game.instance.PoolManager,
                    "Blow",
                    () => Instantiate(Blow, Game.instance.PoolManager.transform, false)
                );
                blow.name = "Blow";
                Blow script = blow.GetComponent<Blow>();
                script.Reset(weapon.stats, direction);
                yield return new WaitForSeconds(weapon.stats.Life);
                blow.SetActive(false);
            }
        }
    }

    private IEnumerator Lilpaaaaaa()
    {
        // TODO: attack animation
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            GameObject ring = ObjectPool.Get(
                Game.instance.PoolManager,
                "Ring",
                () => Instantiate(Ring, Game.instance.PoolManager.transform, false)
            );
            ring.name = "Ring";
            ring.transform.position = Player.instance.transform.position - Vector3.forward * 0.8f;
            List<GameObject> enemy = Scanner.ScanAll(Player.instance.transform.position, 6, "Enemy");
            for(int i = 0; i < enemy.Count; i++)
            {
                EnemyPool enemyPool = EnemyManager.instance.GetEnemy(enemy[i]);
                int deal = weapon.stats.Power + i * weapon.stats.DecreasePower;
                enemyPool.health -= deal;
                Damage.instance.WriteDamage(enemyPool.target, deal);
            }
            yield return new WaitForSeconds(weapon.stats.Life);
            ring.SetActive(false);
        }
    }

    private IEnumerator StampPlump()
    {
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            GameObject enemy = Scanner.Scan(Player.instance.transform.position, 8, "Enemy");
            if(enemy)
            {
                GameObject magicCircle = Instantiate(MagicCircle, enemy.transform);
                magicCircle.GetComponent<MagicCircle>().Reset(weapon.stats, enemy);
            }
            yield return wait;
        }
    }

    private IEnumerator Headpin()
    {
        List<GameObject> targets = new(){};
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            for(int i = 0; i < weapon.stats.Through; i++)
            {
                GameObject enemy = Scanner.ScanFilter(Player.instance.transform.position, 14, "Enemy", targets);
                if(enemy == null) continue;
                GameObject headpin = ObjectPool.Get(
                    Game.instance.PoolManager,
                    "Headpin",
                    () => Instantiate(HeadpinPrefab, Game.instance.PoolManager.transform, false)
                );
                headpin.name = "Headpin";
                headpin.transform.position = enemy.transform.position;
                targets.Add(enemy);
                headpin.GetComponent<Headpin>().Reset(weapon.stats, enemy, targets);
            }
        }
    }

    private Quaternion LookAtTarget(Vector2 target)
    {
        Vector2 distance = transform.position.x < target.x
            ? (Vector3)target - transform.position
            : transform.position - (Vector3)target
        ;
        return Quaternion.AngleAxis((float)(Math.Atan2(distance.y, distance.x) * Mathf.Rad2Deg), Vector3.forward);
    }
}
