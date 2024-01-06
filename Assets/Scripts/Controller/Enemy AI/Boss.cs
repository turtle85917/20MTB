using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Boss : EnemyAIStruct
{
    [SerializeField] private bool isForceIdle;

    public new void OnDie()
    {
        UIManager.instance.ShowGameClearPanel();
    }

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        FlipObject();

        if(isForceIdle) animator.SetBool("isWalk", false);
        else animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle);

        if(enemyPool.health <= 0)
        {
            isDied = true;
            StartCoroutine(IECameraManage());
        }
    }

    IEnumerator IECameraManage()
    {
        GetComponent<SortingGroup>().enabled = false;

        Vector2 targetPosition = transform.position;
        Game.cameraAgent.status = CameraAgent.Status.DieBoss;
        const float duration = 2.5f;
        float exitTime = Time.unscaledTime + duration;
        while(exitTime >= Time.unscaledTime)
        {
            Vector2 position = Vector2.Lerp(Camera.main.transform.position, targetPosition, duration * Time.deltaTime);
            Camera.main.transform.position = new Vector3(position.x, position.y, -10);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 2, duration * Time.deltaTime);
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.5f, duration * Time.deltaTime);
            yield return null;
        }
        animator.SetTrigger("isDied");
    }
}
