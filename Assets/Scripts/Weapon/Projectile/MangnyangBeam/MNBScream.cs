using UnityEngine;

public class MNBScream : MonoBehaviour
{
    private SpriteRenderer sprite;
    private WeaponState weaponState;
    private enum WeaponState
    {
        Show,
        Hide
    }

    public void Init()
    {
        sprite.color = Color.white;
        transform.localScale = Vector3.zero;
        weaponState = WeaponState.Show;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(weaponState == WeaponState.Show)
        {
            if(transform.localScale.y >= 2.6f)
            {
                weaponState = WeaponState.Hide;
                return;
            }
            transform.localScale += Vector3.one * 0.05f;
        }
        if(weaponState == WeaponState.Hide)
        {
            sprite.color -= new Color(0, 0, 0, 0.02f);
        }
    }
}
