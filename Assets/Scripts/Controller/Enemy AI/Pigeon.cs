using System.Linq;
using _20MTB.Utillity;
using UnityEngine;

public class Pigeon : EnemyAIStruct
{
    private GameObject @object;
    private GameObject lastObject;

    private float findedAt;

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        base.Update();
        if(@object?.activeSelf == false) @object = null;
        if(lastObject?.activeSelf == false) lastObject = null;
        if(@object != null && Vector2.Distance(@object.transform.position, transform.position) > 3f) @object = null;
        if(@object == null || Time.time - findedAt > 5f)
        {
            CastLayerdObject();
            findedAt = Time.time;
        }
        transform.rotation = Quaternion.AngleAxis(@object?.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
        animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle && @object != null);

        if(@object != null && Scanner.IsAnyTargetAround(transform.position, 2f, @object))
        {
            if((@object.CompareTag("Player") || @object.CompareTag("Enemy")) && Time.time - findedAt > 1f)
            {
                AttackManager.AttackTarget(4, @object, enemyPool);
            }
            @object = null;
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
    }

    private void CastLayerdObject()
    {
        if(Random.value < 0.3)
        {
            @object = null;
            return;
        }
        RaycastHit2D[] raycasts = Physics2D.CircleCastAll(transform.position, 3f, Vector2.right, 0, 64).Where(item => item.collider.gameObject.activeSelf).ToArray();
        GameObject result = null;
        float minDistance = 0;
        foreach(RaycastHit2D raycast in raycasts)
        {
            if(raycast.collider.name == "Pigeon") continue;
            if(lastObject?.Equals(raycast.collider.gameObject) == true) return;
            float distance = Vector3.Distance(transform.position, raycast.transform.position);
            if(minDistance == 0 || distance < minDistance)
            {
                result = raycast.collider.gameObject;
                minDistance = distance;
            }
        }
        @object = result;
        if(result != null) lastObject = result;
    }
}
