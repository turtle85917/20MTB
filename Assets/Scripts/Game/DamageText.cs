using System.Collections;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private void Update()
    {
        if(gameObject.activeSelf)
        {
            transform.localPosition += Vector3.up * 60 * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
