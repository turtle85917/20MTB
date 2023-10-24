using _20MTB.Stats;
using UnityEngine;

[CreateAssetMenu(menuName = "캐릭터")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public string defaultWeapon;
    public Sprite headImage;
    public RuntimeAnimatorController controller;
    public Stats stats;
}
