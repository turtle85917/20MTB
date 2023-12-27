using _20MTB.Utillity;
using UnityEngine;

public class Germ : EnemyAIStruct
{
    private Quaternion quaternion;

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        base.Update();
        if(
            -GameUtils.maxPosition.x > transform.position.x || transform.position.x > GameUtils.maxPosition.x ||
            -GameUtils.maxPosition.y > transform.position.y || transform.position.y > GameUtils.maxPosition.y
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
            if(Random.value < 0.2f) return;
            AttackManager.AttackTarget(3, raycast.collider.gameObject, null);
            AttackManager.AttackTarget(4, gameObject, null);
            break;
        }
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        quaternion = Quaternion.AngleAxis(Random.Range(-360f, 360f), Vector3.forward);
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
