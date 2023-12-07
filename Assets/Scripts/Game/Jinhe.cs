using System.Collections;
using UnityEngine;

public class Jinhe : MonoBehaviour
{
    public float life {private get; set;}        // 진희 생존 시간
    public GameObject weaponUser;                // 사용할 무기를 사용하고 있는 대상
    public EnemyPool weaponOwner;   // 무기 원래 사용자 (적 전용)
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

    public void Init()
    {
        rigid.velocity = Vector2.zero;
        StartCoroutine(Dead());
    }

    public void OnBrokenWeapon()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sprite.flipX = transform.position.x < weaponUser.transform.position.x;
    }

    private void FixedUpdate()
    {
        // 자기 편을 따라감
        Vector3 distance = weaponUser.transform.position - transform.position;
        rigid.MovePosition(Vector3.MoveTowards(rigid.position, weaponUser.transform.position - distance.normalized * 3, 15 * Time.fixedDeltaTime));
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(life);
        gameObject.SetActive(false);
    }
}
