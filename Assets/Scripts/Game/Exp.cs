using UnityEngine;

public class Exp : MonoBehaviour
{
    public int exp {private get; set;}
    private Rigidbody2D rigid;
    private Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Scanner.Scan(transform.position, 1.4f, "Player") != null)
        {
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, Player.@object.transform.position, 40 * Time.deltaTime));
        }
    }

    private void OnEnable()
    {
        anim.Play("Exp_Idle");
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
