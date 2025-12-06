using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("请把挂着 PlayerDirectionalDash 脚本的物体拖进来")]
    public PlayerDirectionalDash dashScript; // 新增：引用冲刺脚本
    // public PlayerMovement playerMovementScript; // (保留你之前的角色移动脚本引用)
    public ModifierTool modifierScript;

    [Header("设置")]
    public GameObject previewObject;
    public GameObject blockPrefab;
    public float rotateSpeed = 100f;

    private bool isBuilding = false;

    void Start()
    {
        TurnOff(); // 游戏开始时确保关闭
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!isBuilding)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }

        if (isBuilding && previewObject != null)
        {
            FollowMouse();
            HandleRotation();
            HandleBuilding();
        }
    }

    // 新增：开启模式
    public void TurnOn()
    {
        isBuilding = true;
        if (previewObject != null) previewObject.SetActive(true);

        // 关键点：开启建造时，强制关闭冲刺模式！
        if (dashScript != null) dashScript.TurnOff();
        if (modifierScript != null) modifierScript.TurnOff();

        // 锁定角色移动 (如果你之前写了这部分)
        // if(playerMovementScript != null) playerMovementScript.canMove = false;
    }

    // 新增：关闭模式
    public void TurnOff()
    {
        isBuilding = false;
        if (previewObject != null) previewObject.SetActive(false);

        // 恢复角色移动
        // if(playerMovementScript != null) playerMovementScript.canMove = true;
    }

    void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        previewObject.transform.position = mousePos;
    }

    void HandleRotation()
    {
        float rotationAmount = 0f;
        if (Input.GetKey(KeyCode.A)) rotationAmount = 1f;
        else if (Input.GetKey(KeyCode.D)) rotationAmount = -1f;
        previewObject.transform.Rotate(0, 0, rotationAmount * rotateSpeed * Time.deltaTime);
    }

    void HandleBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(blockPrefab, previewObject.transform.position, previewObject.transform.rotation);

            // 解决你的第一个问题：生成完后，立即关闭自己
            TurnOff();
        }
    }
}
