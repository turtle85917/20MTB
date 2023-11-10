using UnityEngine;

public class Sledgehammer : BaseWeapon
{
    [SerializeField] private GameObject Rock;
    private int direction;
    private Vector2 maxPosition;
    private int GetAngleZ()
    {
        int z = Mathf.FloorToInt(transform.eulerAngles.z);
        return z == 0 ? z : direction == -1 ? 360 - z : z;
    }

    public new void Init()
    {
        base.Init();
        direction = sprite.flipX ? -1 : 1;
        maxPosition = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
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
            for(int i = 0; i < 20; i++)
            {
                GameObject rock = ObjectPool.Get(Game.PoolManager, Rock.name, Rock);
                Vector3 cameraPosition = Camera.main.transform.position;
                rock.transform.position = new Vector3(Random.Range(-maxPosition.x + cameraPosition.x, maxPosition.x + cameraPosition.x), maxPosition.y + cameraPosition.y);
                rock.transform.localScale = Vector2.one * Random.Range(0.3f, 1f);
                rock.transform.GetComponent<Rigidbody2D>().gravityScale = Random.Range(1f, 4f);
                Rock script = rock.GetComponent<Rock>();
                script.index = i + 1;
                script.stats = stats;
                script.weaponUser = weaponUser;
            }
        }
    }
}
