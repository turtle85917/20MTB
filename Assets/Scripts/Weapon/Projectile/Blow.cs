using UnityEngine;

public class Blow : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        animation.Play("Show");
        Vector2 direction = new Vector2((Camera.main.ScreenToWorldPoint(Input.mousePosition) - Game.Player.transform.position).normalized.x < 0 ? -1 : 1, 0);
        sprite.flipX = direction.x < 0;
        transform.localPosition = Game.Player.transform.position + Vector3.right * direction.x * 1.2f;
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && penetrate < stats.Penetrate)
        {
            AttackManager.AttackTarget(weaponId, other.gameObject, penetrate);
            penetrate++;
        }
    }
}
