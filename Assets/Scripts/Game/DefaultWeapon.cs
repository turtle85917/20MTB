using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _20MTB.Utillity;
using UnityEngine;

public class DefaultWeapon : MonoBehaviour
{
    [SerializeField] private GameObject Blow;
    [SerializeField] private GameObject Star;
    [SerializeField] private GameObject Scream;
    [SerializeField] private GameObject MagicCircle;
    [SerializeField] private GameObject HeadpinPrefab;
    [SerializeField] private GameObject Diagums;
    private Weapon weapon;
    private WaitForSeconds cooltimeWait;

    private void Start()
    {
        weapon = WeaponBundle.GetWeapon(Player.playerData.data.defaultWeapon);
        cooltimeWait = new WaitForSeconds(weapon.stats.Cooldown);
        switch(weapon.weapon.WeaponId)
        {
            case "Wakchori":
                StartCoroutine(WeaponCycle(
                    Game.PoolManager,
                    Blow,
                    (blow) => {
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        blow.transform.rotation = GameUtils.LookAtTarget(blow.transform.position, mousePosition);
                        Blow script = blow.GetComponent<Blow>();
                        script.Init();
                        script.through = 0;
                        script.stats = weapon.stats;
                        script.direction = mousePosition - (Vector2)blow.transform.position;
                    }
                ));
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
            case "DiaGum":
                StartCoroutine(DiaGum());
                break;
        }
    }

    private IEnumerator Wakchori()
    {
        while(true)
        {
            yield return cooltimeWait;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 distance = Game.Player.transform.position - (Vector3)mousePosition;
            GameObject blow = ObjectPool.Get(
                Game.PoolManager,
                "Blow",
                (parent) => Instantiate(Blow, parent.transform, false)
            );
            // blow.transform.rotation = GameUtils.LookAtTarget(Vector2.zero, mousePosition);
            Blow script = blow.GetComponent<Blow>();
            // script.Reset(weapon.stats, distance.normalized * -1);
            yield return new WaitForSeconds(weapon.stats.Life);
            blow.SetActive(false);
        }
    }

    private IEnumerator MagicWand()
    {
        List<GameObject> targets = new List<GameObject>(){};
        while(true)
        {
            yield return cooltimeWait;
            for(int i = 0; i < weapon.stats.ProjectileCount; i++)
            {
                GameObject star = ObjectPool.Get(
                    Game.PoolManager,
                    "Star",
                    (parent) => Instantiate(Star, parent.transform, false)
                );
                star.name = "Star";
                star.transform.localPosition = Game.Player.transform.position;
                star.GetComponent<Star>().Reset(weapon.stats, targets);
            }
        }
    }

    private IEnumerator MuayThai()
    {
        while(true)
        {
            yield return cooltimeWait;
            List<GameObject> enemy = Scanner.ScanAll(Game.Player.transform.position, 10, "Enemy");
            // Player.instance.Attack();
            Vector2 direction = Player.lastDirection;
            if(enemy.Count > 0)
            {
                enemy = enemy.OrderBy(item => Vector3.Distance(item.transform.position, Game.Player.transform.position)).ToList();
                for(int i = 0; i < weapon.stats.Through; i++)
                {
                    if(enemy.Count <= i) break;
                    var enemyPool = EnemyManager.GetEnemy(enemy[i]);
                    EnemyManager.AttackEnemy(enemy[i], weapon.stats, i);
                }
            }
            else
            {
                GameObject blow = ObjectPool.Get(
                Game.PoolManager,
                    "Blow",
                    (parent) => Instantiate(Blow, parent.transform, false)
                );
                Blow script = blow.GetComponent<Blow>();
                script.Init();
                yield return new WaitForSeconds(weapon.stats.Life);
                blow.SetActive(false);
            }
        }
    }

    private IEnumerator Lilpaaaaaa()
    {
        while(true)
        {
            yield return cooltimeWait;
            // Player.instance.Attack();
            GameObject scream = ObjectPool.Get(
                Game.PoolManager,
                "Scream",
                (parent) => Instantiate(Scream, parent.transform, false)
            );
            scream.transform.position = Game.Player.transform.position;
            Scream script = scream.GetComponent<Scream>();
            script.Reset(weapon.stats);
            yield return new WaitForSeconds(weapon.stats.Life);
            scream.SetActive(false);
        }
    }

    private IEnumerator StampPlump()
    {
        while(true)
        {
            GameObject enemy = Scanner.Scan(Game.Player.transform.position, 8, "Enemy");
            if(enemy)
            {
                GameObject magicCircle = Instantiate(MagicCircle, enemy.transform);
                magicCircle.GetComponent<MagicCircle>().Reset(weapon.stats, enemy);
            }
            yield return cooltimeWait;
        }
    }

    private IEnumerator Headpin()
    {
        List<GameObject> targets = new List<GameObject>(){};
        while(true)
        {
            yield return cooltimeWait;
            for(int i = 0; i < weapon.stats.Through; i++)
            {
                GameObject enemy = Scanner.ScanFilter(Game.Player.transform.position, 14, "Enemy", targets);
                if(enemy == null) continue;
                GameObject headpin = ObjectPool.Get(
                    Game.PoolManager,
                    "Headpin",
                    (parent) => Instantiate(HeadpinPrefab, parent.transform, false)
                );
                headpin.transform.position = enemy.transform.position;
                targets.Add(enemy);
                headpin.GetComponent<Headpin>().Reset(weapon.stats, enemy, targets);
            }
        }
    }

    private IEnumerator DiaGum()
    {
        while(true)
        {
            yield return cooltimeWait;
            GameObject diagums = ObjectPool.Get(
                Game.PlayerWeapons,
                "DiaGums",
                (parent) => Instantiate(Diagums, parent.transform, false)
            );
            Diagums script = diagums.GetComponent<Diagums>();
            script.Reset(weapon.stats);
            yield return new WaitForSeconds(weapon.stats.Life);
            diagums.SetActive(false);
        }
    }

    private IEnumerator WeaponCycle(GameObject Parent, GameObject WeaponPrefab, Action<GameObject> createFunc)
    {
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject obj = ObjectPool.Get(Parent, WeaponPrefab.name, (parent) => Instantiate(WeaponPrefab, parent.transform, false));
            createFunc(obj);
            yield return new WaitForSeconds(weapon.stats.Life);
            obj.SetActive(false);
        }
    }
}
