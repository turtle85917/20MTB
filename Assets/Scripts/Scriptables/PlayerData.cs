using UnityEngine;

[CreateAssetMenu(menuName = "캐릭터")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public ItemData defaultWeapon;
    public Sprite headImage;
    public RuntimeAnimatorController controller;
    public Stats stats;
}
