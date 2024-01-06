using System.Collections;
using UnityEngine;

public class Sledgehammer : BaseWeapon
{
    [SerializeField] private GameObject Rock;
    private int direction;
    private int GetAngleZ()
    {
        int z = Mathf.FloorToInt(transform.eulerAngles.z);
        return z == 0 ? z : direction == -1 ? 360 - z : z;
    }

    public new void Init()
    {
        base.Init();
        direction = sprite.flipX ? -1 : 1;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(weaponUser.CompareTag("Player"))
        {
            if(GetAngleZ() < 90)
            {
                transform.rotation *= Quaternion.AngleAxis(5 * direction, Vector3.forward);
                transform.position = weaponUser.transform.position + transform.up;
            }
            if(GetAngleZ() == 90 && weaponStatus == WeaponStatus.Idle)
            {
                weaponStatus = WeaponStatus.GoAway;
                Game.cameraAgent.Shake(0.2f);
                StartCoroutine(IEHide());
                int rockCount = 20;
                for(int i = 0; i < rockCount; i++)
                {
                    GameObject rock = ObjectPool.Get(Game.PoolManager, Rock.name, Rock);
                    Vector3 cameraPosition = Camera.main.transform.position;
                    float rockX = Random.Range(-Game.maxPosition.x + cameraPosition.x, Game.maxPosition.x + cameraPosition.x);
                    if(weaponUser.CompareTag("Enemy")) rockX = Player.@object.transform.position.x + Random.Range(-3f, 3f);

                    rock.transform.position = new Vector3(rockX, Game.maxPosition.y + cameraPosition.y);
                    rock.transform.localScale = Vector2.one * Random.Range(0.3f, 1f);
                    rock.transform.GetComponent<Rigidbody2D>().gravityScale = Random.Range(1f, 5f);

                    Rock script = rock.GetComponent<Rock>();
                    script.index = i + 1;
                    script.stats = stats;
                    script.weaponUser = weaponUser;
                }
            }
        }
        if(weaponUser.CompareTag("Enemy"))
        {
            sprite.flipX = true;
            transform.rotation *= Quaternion.Euler(Vector3.forward * 7);
            transform.position = weaponUser.transform.position + transform.up * 2 + Vector3.right * -0.25f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(weaponUser.CompareTag("Enemy") && other.gameObject.CompareTag("Player"))
        {
            AttackManager.AttackTarget(7, Player.@object, EnemyManager.GetEnemy(weaponUser));
        }
    }

    IEnumerator IEHide()
    {
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
        StealEnemyWeapon();
    }
}
