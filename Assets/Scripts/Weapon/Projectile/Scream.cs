using UnityEngine;

public class Scream : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        transform.localScale = new Vector2(1, 1);
        transform.localPosition = Game.Player.transform.position;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Color color = sprite.color;
        color.a -= 0.08f;
        sprite.color = color;
        transform.localScale += new Vector3(0.4f, 0.4f);
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
