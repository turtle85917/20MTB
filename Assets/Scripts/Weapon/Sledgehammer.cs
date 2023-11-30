using UnityEngine;

// TODO: 적이 쓸 경우, 고려하기

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
        if(GetAngleZ() < 90)
        {
            transform.rotation *= Quaternion.AngleAxis(5 * direction, Vector3.forward);
            transform.position = weaponUser.transform.position + transform.up;
        }
        if(GetAngleZ() == 90 && weaponStatus == WeaponStatus.Idle)
        {
            weaponStatus = WeaponStatus.GoAway;
            Game.cameraAgent.Shake(0.2f);
            int rockCount = 20;
            if(weaponUser.CompareTag("Enemy")) rockCount = 5;
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
}
