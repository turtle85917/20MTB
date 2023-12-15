using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class CameraAgent : MonoBehaviour
{
    private Status status;
    private enum Status
    {
        Idle,
        Shaking
    }

    public void PlayPlayerDie()
    {
        Player.@object.GetComponent<Animator>().SetTrigger("isDied");
    }

    public void Shake(float time)
    {
        status = Status.Shaking;
        StartCoroutine(ShakeReset(time));
    }

    public void Reset()
    {
        status = Status.Idle;
        Camera.main.transform.position = GameUtils.FixedPosition();
    }

    private void Update()
    {
        if(status == Status.Shaking)
        {
            Camera.main.transform.position = GameUtils.FixedPosition() + (Vector3)Random.insideUnitCircle * 0.5f;
        }
    }

    private void LateUpdate()
    {
        if(Game.isPaused || status == Status.Shaking) return;
        Camera.main.transform.position = GameUtils.FixedPosition();
    }

    private IEnumerator ShakeReset(float time)
    {
        yield return new WaitForSeconds(time);
        Reset();
    }
}
