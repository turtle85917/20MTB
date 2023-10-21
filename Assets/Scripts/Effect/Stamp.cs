using System.Collections;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    private void Update()
    {
        transform.localPosition += Vector3.down * 2.5f * Time.deltaTime;
    }

    private void OnEnable()
    {
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
