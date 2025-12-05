using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("拖入场景中那个半透明的虚影对象")]
    public GameObject previewObject;

    [Tooltip("拖入要生成的实体长方形 Prefab")]
    public GameObject blockPrefab;

    [Tooltip("旋转速度")]
    public float rotateSpeed = 100f;

    // 内部状态
    private bool isBuilding = false; // 是否处于建造模式

    void Start()
    {
        // 游戏开始确保虚影是隐藏的
        if (previewObject != null)
            previewObject.SetActive(false);
    }

    void Update()
    {
        // 1. 切换模式：按下 '2' 键
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isBuilding = !isBuilding;

            if (previewObject != null)
            {
                previewObject.SetActive(isBuilding);
                // 可选：每次打开时重置角度
                // previewObject.transform.rotation = Quaternion.identity; 
            }
        }

        // 如果处于建造模式，执行跟随、旋转和生成
        if (isBuilding && previewObject != null)
        {
            FollowMouse();
            HandleRotation();
            HandleBuilding();
        }
    }

    // 让虚影跟随鼠标
    void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 确保 Z 轴在 0，不然可能跑到摄像机后面去
        previewObject.transform.position = mousePos;
    }

    // 处理 A/D 旋转
    void HandleRotation()
    {
        float rotationAmount = 0f;

        // 按 A 向左转 (逆时针)，按 D 向右转 (顺时针)
        if (Input.GetKey(KeyCode.A))
        {
            rotationAmount = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationAmount = -1f;
        }

        // 应用旋转: 绕 Z 轴旋转
        // Time.deltaTime 确保旋转速度平滑，不受帧率影响
        previewObject.transform.Rotate(0, 0, rotationAmount * rotateSpeed * Time.deltaTime);
    }

    // 处理点击生成
    void HandleBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 在虚影的位置和旋转角度，生成实体 Prefab
            Instantiate(blockPrefab, previewObject.transform.position, previewObject.transform.rotation);

            // 可选：生成完后是否自动退出建造模式？
            // isBuilding = false;
            // previewObject.SetActive(false);
        }
    }
}
