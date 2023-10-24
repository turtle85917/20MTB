using System.Collections.Generic;
using _20MTB.Utillity;
using UnityEngine;

public class Player : BaseController
{
    public static Vector2 lastDirection {get; private set;}
    public static PlayerStatus playerData {get; private set;}
    private Vector2 inputDirection;
    [SerializeField] private Character character;
    [SerializeField] private PlayerData[] players;
#region 플레이어 데이터
    public class PlayerStatus
    {
        public int health;
        public int level;
        public int exp;
        public List<Weapon> weapons;
        public PlayerData data;
    }
#endregion

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerData data = players[(int)character];
        playerData = new PlayerStatus()
        {
            health = data.stats.MaxHealth,
            level = 0, exp = 0,
            weapons = new List<Weapon>(){},
            data = data
        };
        animator.runtimeAnimatorController = data.controller;
    }

    private void Update()
    {
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");
        animator.SetBool("isWalk", inputDirection.magnitude != 0);
        transform.rotation = new Quaternion(0, inputDirection.x < 0 ? 180 : 0, 0, 0);
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(GameUtils.MovePositionLimited(rigid.position + inputDirection * playerData.data.stats.MoveSpeed * Time.fixedDeltaTime, transform.position.z));
        if(inputDirection.magnitude != 0)
        {
            lastDirection = inputDirection;
        }
    }
}
