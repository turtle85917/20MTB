using System.Collections;
using UnityEngine;

public class Jinhe : MonoBehaviour
{
    public float life {private get; set;}                 // 진희 생존 시간
    [HideInInspector] public GameObject weaponUser;       // 사용할 무기를 사용하고 있는 대상
    private Rigidbody2D rigid;

    public void Init()
    {
        rigid.velocity = Vector2.zero;
        StartCoroutine(Dead());
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
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
