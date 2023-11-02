using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTool : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject[] Contents;

    [Header("Content : Add Weapon")]
    [SerializeField] private TMP_InputField WeaponId;

    [Header("Content : Add Weapon To Enemy")]
    [SerializeField] private GameObject SelectEnemyPanel;
    [SerializeField] private GameObject AddWeaponPanel;
    [SerializeField] private TMP_InputField EnemyWeaponId;
    private GameObject selectTarget;
    private Vector3 lastCameraPosition;
    private Vector3 lastMousePosition;

    private int level;
    private Content openContent;
    private readonly Dictionary<KeyCode, Content> contentShortcut = new Dictionary<KeyCode, Content>
    {
        {KeyCode.I, Content.AddWeapon},
        {KeyCode.O, Content.AddWeaponToEnemy}
    };
    private enum Content
    {
        AddWeapon,
        AddWeaponToEnemy
    }

    private void Update()
    {
        foreach(KeyCode key in contentShortcut.Keys)
        {
            if(Input.GetKeyDown(key) && !Panel.activeSelf)
            {
                Reset();
                Panel.SetActive(true);
                Contents[(int)contentShortcut[key]].SetActive(true);
                openContent = contentShortcut[key];
                if(!Game.isPaused)
                    Game.Pause();
                break;
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Reset();
            Game.Resume();
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            switch(openContent)
            {
                case Content.AddWeapon:
                    WeaponBundle.AddWeaponToTarget(Player.@object, WeaponId.text);
                    WeaponId.text = string.Empty;
                    break;
                case Content.AddWeaponToEnemy:
                    WeaponBundle.AddWeaponToTarget(selectTarget, EnemyWeaponId.text);
                    EnemyWeaponId.text = string.Empty;
                    break;
            }
        }
        switch(openContent)
        {
            case Content.AddWeaponToEnemy:
                switch(level)
                {
                    case 0:
                        SelectEnemyPanel.SetActive(true);
                        AddWeaponPanel.SetActive(false);
                        if(Input.GetMouseButtonDown(0))
                        {
                            lastCameraPosition = Camera.main.transform.position;
                            lastMousePosition = Input.mousePosition;
                            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                            if(hit && hit.collider.CompareTag("Enemy"))
                            {
                                level = 1;
                                selectTarget = hit.collider.gameObject;
                            }
                        }
                        if(Input.GetMouseButton(0))
                        {
                            Vector3 fixedMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - lastMousePosition) - Camera.main.ScreenToWorldPoint(Vector3.zero); // Vecotr3.zero는 원점이므로 시작 위치임.
                            Camera.main.transform.position = lastCameraPosition - new Vector3(fixedMousePosition.x, fixedMousePosition.y, 0);
                        }
                        break;
                    case 1:
                        if(selectTarget == null) level = 0;
                        SelectEnemyPanel.SetActive(false);
                        AddWeaponPanel.SetActive(true);
                        break;
                }
                break;
        }
    }

    private void Reset()
    {
        level = 0;
        selectTarget = null;
        Camera.main.transform.position = Player.@object.transform.position + Vector3.back * 10;
        Panel.SetActive(false);
        foreach(GameObject Content in Contents)
            Content.SetActive(false);
    }
}
