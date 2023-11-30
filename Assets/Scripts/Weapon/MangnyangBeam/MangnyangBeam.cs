using _20MTB.Utillity;
using UnityEngine;

public class MangnyangBeam : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        animation.Play("Idle");
        float size = (GetScreenWallPos() - (Vector2)weaponUser.transform.position).magnitude;
        transform.localScale = new Vector3(1, size, 1);
        transform.localPosition = transform.up * size;
        Game.cameraAgent.Shake(0.3f);
    }

    public void HideCore()
    {
        transform.parent.GetComponentInChildren<Animator>().SetTrigger("isHide");
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUser)))
        {
            penetrate++;
            AttackManager.AttackTarget("MangnyangBeam", other.gameObject, penetrate, source:weaponUser);
        }
    }

    private Vector2 GetScreenWallPos()
    {
        float zAngle = transform.eulerAngles.z;
        if(60 >= zAngle || zAngle - 360 >= -60) return new Vector2(0, Game.maxPosition.y);
        else if(300 >= zAngle && zAngle > 240) return new Vector2(Game.maxPosition.x, 0);
        else if(120 <= zAngle && zAngle <= 240) return new Vector2(0, -Game.maxPosition.y);
        else return new Vector2(-Game.maxPosition.x, 0);
    }
}
