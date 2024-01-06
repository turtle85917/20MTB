using _20MTB.Utillity;
using UnityEngine;

public class Cex : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        sprite.color = Color.white;
        transform.localScale = Vector2.one * stats.Range;
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUser)))
        {
            Affecter script = other.GetComponent<Affecter>();
            script.StartCoroutine(script.ThreeComboKnockback(weaponUser));
            count++;
        }
    }
}
