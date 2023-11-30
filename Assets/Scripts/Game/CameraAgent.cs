using System.Collections;
using UnityEngine;

public class CameraAgent : MonoBehaviour
{
    private Status status;
    private enum Status
    {
        Idle,
        Shaking
    }

    public void Shake(float time)
    {
        status = Status.Shaking;
        StartCoroutine(ShakeReset(time));
    }

    public void Reset()
    {
        status = Status.Idle;
        Camera.main.transform.position = FixedPosition();
    }

    private void Update()
    {
        if(status == Status.Shaking)
        {
            Camera.main.transform.position = FixedPosition() + (Vector3)Random.insideUnitCircle * 0.5f;
        }
    }

    private void LateUpdate()
    {
        if(Game.isPaused || status == Status.Shaking) return;
        Camera.main.transform.position = FixedPosition();
    }

    private IEnumerator ShakeReset(float time)
    {
        yield return new WaitForSeconds(time);
        Reset();
    }

    private Vector3 FixedPosition() => new Vector3(Player.@object.transform.position.x, Player.@object.transform.position.y, -10);
}
