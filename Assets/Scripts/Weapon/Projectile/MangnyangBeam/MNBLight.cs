using System.Collections;
using UnityEngine;

public class MNBLight : MonoBehaviour
{
    private float maxSize;
    private WeaponStatus weaponStatus;
    private enum WeaponStatus
    {
        Expand,
        Stay,
        Cutback
    }

    public void Init()
    {
        maxSize = Random.Range(1.8f, 2f);
        weaponStatus = WeaponStatus.Expand;
    }

    private void Update()
    {
        if(weaponStatus == WeaponStatus.Expand)
        {
            if(transform.localScale.y >= maxSize)
            {
                weaponStatus = WeaponStatus.Stay;
                StartCoroutine(WaitedHide());
                return;
            }
            transform.localScale += Vector3.up * 0.04f;
            transform.localPosition = transform.up * transform.localScale.y;
        }
        if(weaponStatus == WeaponStatus.Cutback)
        {
            if(transform.localScale.x <= 0)
            {
                gameObject.SetActive(false);
                return;
            }
            transform.localScale -= Vector3.right * 0.05f;
            transform.localPosition = transform.up * transform.localScale.y;
        }
    }

    private IEnumerator WaitedHide()
    {
        yield return new WaitForSeconds(0.4f);
        weaponStatus = WeaponStatus.Cutback;
    }
}
