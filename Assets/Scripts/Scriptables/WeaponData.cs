using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "무기")]
public class WeaponData : ScriptableObject
{
    public string WeaponId;
    public Sprite logo;
    public MonoScript weaponCycleScriptFile;
    public MonoScript weaponExecuteScriptFile;
    public Object[] resources;
}
