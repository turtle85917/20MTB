using UnityEngine;

public class Axe : MonoBehaviour, IExecuteWeapon
{
    public void ExecuteWeapon()
    {
        Debug.Log("도끼 발사띠");
    }

    public void ExecuteEnemyWeapon()
    {
    }
}
