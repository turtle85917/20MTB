using System.Collections;
using UnityEngine;

public class CameraAgent : MonoBehaviour
{
    private Vector3 lastPosition;
    private Status status;
    private enum Status
    {
        Idle,
        Shaking
    }

    public void Shake(float time = 0.5f)
    {
        status = Status.Shaking;
        lastPosition = Camera.main.transform.position;
        if(time != Mathf.Infinity)
            StartCoroutine(ShakeReset(time));
    }

    public void Reset()
    {
        status = Status.Idle;
        Camera.main.transform.position = lastPosition;
    }

    private void Update()
    {
        if(status == Status.Shaking)
        {
            Camera.main.transform.position = lastPosition + (Vector3)Random.insideUnitCircle;
        }
    }

    private void LateUpdate()
    {
        if(Game.isPaused || status == Status.Shaking) return;
        Camera.main.transform.position = Player.@object.transform.position + Vector3.back * 10;
    }

    private IEnumerator ShakeReset(float time)
    {
        yield return new WaitForSeconds(time);
        Reset();
    }
}
