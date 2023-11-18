using UnityEngine;

public class MNBBeam : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        animation.Play("Idle");
        float size = (GetScreenWallPos() - (Vector2)weaponUser.transform.position).magnitude;
        transform.localScale = new Vector3(1, size, 1);
        transform.localPosition = transform.up * size;
        Game.cameraAgent.Shake(Mathf.Infinity);
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
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
