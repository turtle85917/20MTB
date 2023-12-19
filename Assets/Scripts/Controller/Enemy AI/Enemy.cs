using UnityEngine;

public class Enemy : EnemyAIStruct
{
    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        base.Update();
        animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle);
        transform.rotation = Quaternion.AngleAxis(Player.@object.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
    }
}
