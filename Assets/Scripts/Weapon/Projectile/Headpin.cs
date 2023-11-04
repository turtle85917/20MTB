using UnityEngine;

public class Headpin : BaseWeapon
{
    private Vector2 maxPosition;
    private Vector3 cameraPosition;

    public new void Init()
    {
        base.Init();
        transform.localPosition = Player.@object.transform.localPosition;
        transform.rotation = Quaternion.AngleAxis(Random.Range(-360f, 360f), Vector3.forward);
        maxPosition = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
    }

    private void Update()
    {
        cameraPosition = Camera.main.transform.position;
        if(
            -maxPosition.x + cameraPosition.x > transform.position.x || transform.position.x > maxPosition.x + cameraPosition.x ||
            -maxPosition.y + cameraPosition.y > transform.position.y || transform.position.y > maxPosition.y + cameraPosition.y
        )
        {
            UpdateRotation();
        }
        else
        {
            transform.position += transform.right * stats.ProjectileSpeed * Time.deltaTime;
        }
    }

    private void OnBecameInvisible()
    {
        Vector2 teleportVec = Vector2.zero;
        teleportVec.x = Random.Range(0, 1) == 1 ? -maxPosition.x + cameraPosition.x : maxPosition.x + cameraPosition.x;
        teleportVec.y = Random.Range(0, 1) == 1 ? -maxPosition.y + cameraPosition.y : maxPosition.y + cameraPosition.y;
        transform.position = teleportVec;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            AttackManager.AttackTarget("Headpin", other.gameObject, penetrate, (affecter) => affecter.Knockback(gameObject));
            penetrate++;
        }
    }

    private void UpdateRotation()
    {
        transform.position -= transform.right * 0.5f;
        if(90 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 270)
        {
            transform.rotation *= Quaternion.AngleAxis(Random.Range(180f, 270f), Vector3.forward);
        }
        else
        {
            transform.rotation *= Quaternion.AngleAxis(Random.Range(90f, 180f), Vector3.back);
        }
    }
}
