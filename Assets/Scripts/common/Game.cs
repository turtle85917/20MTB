using UnityEngine;

public class Game : MonoBehaviour
{
    public Character character;
    public PlayerData playerData;
    public static Game instance {get; private set;}
    [SerializeField] private PlayerData[] players;

    private void Awake()
    {
        instance = this;
        playerData = players[(int)character];
    }
}
