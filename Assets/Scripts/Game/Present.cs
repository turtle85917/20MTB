using UnityEngine;

public class Present : MonoBehaviour
{
    public PresentType presentType;
    public int exp {private get; set;}
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Scanner.Scan(transform.position, 1.4f, "Player")?.Equals(Player.@object) == true)
        {
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, Player.@object.transform.position, 40 * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            switch(presentType)
            {
                case PresentType.Exp:
                    Player.playerData.exp += exp;
                    break;
                case PresentType.DonatedBox:
                    Game.instance.donatedBoxPanel.Open();
                    break;
            }
        }
    }
}
