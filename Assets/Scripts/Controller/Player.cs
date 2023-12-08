using System.Collections.Generic;
using _20MTB.Utillity;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus
{
    public int health;
    public int level;
    public int exp;
    public List<Weapon> weapons;
    public PlayerData data;
}

public class Player : BaseController
{
    public static Vector2 lastDirection {get; private set;}
    public static PlayerStatus playerData {get; private set;}
    public static GameObject @object;
    public static GameObject weapons;
    private Vector2 inputDirection;
    [Header("플레이어 스크립터블")]
    [SerializeField] private Character character;
    [SerializeField] private PlayerData[] players;
    [Header("UI")]
    [SerializeField] private Image headImage;

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        @object = gameObject;
        weapons = GameObject.FindWithTag("Weapons");
        PlayerData data = players[(int)character];
        playerData = new PlayerStatus()
        {
            health = data.stats.MaxHealth,
            level = 0, exp = 0,
            weapons = new List<Weapon>(){},
            data = data
        };
        animator.runtimeAnimatorController = data.controller;
        WeaponBundle.AddWeaponToTarget(gameObject, data.defaultWeapon);
    }

    public override void OnDie()
    {
        UIManager.instance.GameOverPanel.SetActive(true);
        UIManager.instance.GameOverPanel.GetComponent<Animation>().Play("GameOverPanel_Show");
    }

    private void Update()
    {
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");
        animator.SetBool("isWalk", inputDirection.magnitude != 0);
        transform.rotation = Quaternion.AngleAxis(lastDirection.x < 0 ? 180 : 0, Vector3.up);
    }

    private void FixedUpdate()
    {
        // 플레이어는 어떠한 영향도 받을 수 없는 무적이다.
        rigid.MovePosition(GameUtils.MovePositionLimited(rigid.position + inputDirection * playerData.data.stats.MoveSpeed * Time.fixedDeltaTime));
        if(inputDirection.magnitude != 0)
        {
            lastDirection = inputDirection;
        }
    }
}
