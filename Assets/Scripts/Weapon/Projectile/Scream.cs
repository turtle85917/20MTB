using UnityEngine;

public class Scream : BaseWeapon
{
    public new void Init()
    {
        base.Init();
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
        color.a -= 0.02f;
        sprite.color = color;
        transform.localScale += new Vector3(0.15f, 0.15f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && penetrate < stats.Penetrate)
        {
            AttackManager.AttackTarget("Lilpaaaaaa", other.gameObject, penetrate);
            penetrate++;
        }
    }
}
