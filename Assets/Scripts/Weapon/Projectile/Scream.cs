using UnityEngine;

public class Scream : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        transform.localScale = new Vector2(1, 1);
        transform.localPosition = Player.@object.transform.position;
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
        transform.localScale += new Vector3(0.2f, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && penetrate < stats.Penetrate)
        {
            AttackManager.AttackTarget("Scream", other.gameObject, penetrate);
            penetrate++;
        }
    }
}
