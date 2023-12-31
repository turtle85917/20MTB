using System.Collections;
using TMPro;
using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField] private TMP_Text AlertContent;
    private new Animation animation;

    public void ShowAlert(string content)
    {
        AlertContent.text = content;
        gameObject.SetActive(true);
        animation.Play("Alert_Show");
        StartCoroutine(DelayedHide());
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    private void Start()
    {
        OnHide();
    }

    private IEnumerator DelayedHide()
    {
        yield return new WaitForSeconds(0.4f);
        animation.Play("Alert_Hide");
    }
}
