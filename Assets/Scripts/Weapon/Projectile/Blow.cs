using UnityEngine;

public class Blow : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        animation.Play("Show");
        sprite.flipX = Player.lastDirection.x < 0;
        int direction = (int)Player.lastDirection.x;
        if(direction == 0) direction = 1;
        transform.localPosition = Player.@object.transform.position + Vector3.right * direction * 1.2f;
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            AttackManager.AttackTarget("Wakchori", other.gameObject, penetrate);
            penetrate++;
        }
    }
}
