using _20MTB.Utillity;
using UnityEngine;

public class Germ : EnemyAIStruct
{
    private Quaternion quaternion;
    private Vector3 maxPosition;
    private bool isMaximumChanged;

    private Vector2 GetMaxPosition(bool isMin)
    {
        Vector2 offset = Vector2.zero;
        if(isMaximumChanged) offset = Camera.main.transform.position;
        return new Vector2(maxPosition.x * (isMin ? -1 : 1) + offset.x, maxPosition.y * (isMin ? -1 : 1) + offset.y);
    }

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        base.Update();
        if(isMaximumChanged && Random.value < 0.005f)
        {
            maxPosition = GameUtils.maxPosition;
            isMaximumChanged = false;
        }
        Vector2 minimumPosition = GetMaxPosition(true);
        Vector2 maximumPosition = GetMaxPosition(false);
        if(
            minimumPosition.x > transform.position.x || transform.position.x > maximumPosition.x ||
            minimumPosition.y > transform.position.y || transform.position.y > maximumPosition.y
        )
        {
            UpdateRotation();
        }
        else
        {
            transform.position += quaternion * Vector3.up * enemyPool.data.stats.MoveSpeed * Time.deltaTime;
        }
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(transform.position, Vector2.zero);
        foreach(RaycastHit2D raycast in raycasts)
        {
            if(raycast.collider.name == "Germ") continue;
            if(!raycast.collider.CompareTag("Enemy")) continue;
            AttackManager.AttackTarget(3, raycast.collider.gameObject, null);
            AttackManager.AttackTarget(4, gameObject, null);
            break;
        }
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        isMaximumChanged = false;
        maxPosition = GameUtils.maxPosition;
        quaternion = Quaternion.AngleAxis(Random.Range(-360f, 360f), Vector3.forward);
    }

    private void OnBecameVisible()
    {
        if(Random.value < 0.6f)
        {
            maxPosition = Game.maxPosition;
            isMaximumChanged = true;
        }
    }
    private void OnBecameInvisible()
    {
        if(isMaximumChanged)
        {
            Vector2 teleportVec = Vector2.zero;
            Vector3 cameraPosition = Camera.main.transform.position;
            teleportVec.x = Random.Range(0, 1) == 1 ? -Game.maxPosition.x + cameraPosition.x : Game.maxPosition.x + cameraPosition.x;
            teleportVec.y = Random.Range(0, 1) == 1 ? -Game.maxPosition.y + cameraPosition.y : Game.maxPosition.y + cameraPosition.y;
            transform.position = teleportVec;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            AttackManager.AttackTarget(5, other.gameObject, enemyPool);
            AttackManager.AttackTarget(3, gameObject, null);
        }
    }

    private void UpdateRotation()
    {
        transform.position -= quaternion * Vector3.up * 0.5f;
        if(90 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 270)
        {
            quaternion *= Quaternion.AngleAxis(Random.Range(180f, 270f), Vector3.forward);
        }
        else
        {
            quaternion *= Quaternion.AngleAxis(Random.Range(90f, 180f), Vector3.back);
        }
    }
}
