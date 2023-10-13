using UnityEngine;

public class Exp : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    private int exp;

    public void SetExp(int expVal)
    {
        exp = expVal;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Scanner.Scan(transform.position, 3, "Player") != null)
        {
            Rigidbody.MovePosition(Vector3.MoveTowards(Rigidbody.position, Player.instance.transform.position, 40 * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Player.exp += exp;
        }
    }
}
