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
            blow.transform.rotation = LookAtMouse();
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
                star.name = "Star" + i;
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
        List<GameObject> targets = new(){};
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            List<GameObject> enemy = Scanner.ScanAll(Player.instance.transform.position, 40, "Enemy", 2);
            for(int i = 0; i < enemy.Count; i++)
            {
                EnemyPool enemyPool = EnemyManager.instance.GetEnemy(enemy[i]);
                // enemyPool.target
            }
        }
    }

    private Quaternion LookAtMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = transform.position.x < mousePosition.x
            ? (Vector3)mousePosition - transform.position
            : transform.position - (Vector3)mousePosition
        ;
        return Quaternion.AngleAxis((float)(Math.Atan2(distance.y, distance.x) * Mathf.Rad2Deg), Vector3.forward);
    }
}
