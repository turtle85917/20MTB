using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Character character;
    public PlayerData playerData;
    public static Game instance {get; private set;}
    [SerializeField] private PlayerData[] players;
    [SerializeField] private Image HeadImage;

    private void Awake()
    {
        instance = this;
        playerData = players[(int)character];
    }

    private void Start()
    {
        HeadImage.sprite = playerData.headImage;
    }
}
