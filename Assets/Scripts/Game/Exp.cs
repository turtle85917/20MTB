using UnityEngine;

public class Exp : MonoBehaviour
{
    public int exp {private get; set;}
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Scanner.Scan(transform.position, 3, "Player") != null)
        {
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, Player.@object.transform.position, 40 * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Player.playerData.exp += exp;
        }
    }
}
