using System.Linq;
using _20MTB.Utillity;
using UnityEngine;

public class Pigeon : EnemyAIStruct
{
    private GameObject @object;
    private float findedAt;

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        base.Update();
        if(@object?.activeSelf != true) @object = null;
        if(@object != null && Vector2.Distance(@object.transform.position, transform.position) > 5f) @object = null;
        if(@object == null || Time.time - findedAt > 1f)
        {
            CastLayerdObject();
            findedAt = Time.time;
        }
        transform.rotation = Quaternion.AngleAxis(@object?.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
        animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle && @object != null);

        if(@object && Scanner.IsAnyTargetAround(transform.position, 5f * @object.transform.localScale.magnitude, @object))
        {
            @object = null;
            findedAt = Time.time - 10f;
            AttackManager.AttackTarget(2, @object, enemyPool);
        }
    }

    private void FixedUpdate()
    {
        if(Game.isGameOver) return;
        if(@object != null && !isDied && affecter.status == Affecter.Status.Idle)
        {
            Vector3 position = Vector3.MoveTowards(rigid.position, @object.transform.position, enemyPool.moveSpeed.value * Time.fixedDeltaTime);
            rigid.MovePosition(GameUtils.MovePositionLimited(position));
        }
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        @object = null;
        findedAt = Time.time;
    }

    private void CastLayerdObject()
    {
        RaycastHit2D[] raycasts = Physics2D.CircleCastAll(transform.position, 3f, Vector2.right, 0, 64).Where(item => item.collider.gameObject.activeSelf).ToArray();
        GameObject result = null;
        float minDistance = 0;
        foreach(RaycastHit2D raycast in raycasts)
        {
            if(raycast.collider.name == "Pigeon") continue;
            float distance = Vector3.Distance(transform.position, raycast.transform.position);
            if(raycast.collider.gameObject.CompareTag("Player"))
            {
                result = raycast.collider.gameObject;
                break;
            }
            if(minDistance == 0 || distance < minDistance)
            {
                result = raycast.collider.gameObject;
                minDistance = distance;
            }
        }
        @object = result;
    }
}
