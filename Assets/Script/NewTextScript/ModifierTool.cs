using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierTool : MonoBehaviour
{
    [Header("引用")]
    // 记得要在 Inspector 里把另外两个脚本拖进来！
    public PlayerDirectionalDash dashScript;
    public BlockBuilder buildScript;

    [Header("设置")]
    public LayerMask wallLayer; // 确保你的墙体有 Layer (比如 Default 或 Ground)
    public GameObject selectionIndicator; // 可选：鼠标上的一个小图标

    private bool isModifying = false;

    void Start()
    {
        TurnOff();
    }

    void Update()
    {
        // 按 3 切换模式
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!isModifying) TurnOn();
            else TurnOff();
        }

        if (isModifying)
        {
            HandleModification();
        }
    }

    public void TurnOn()
    {
        isModifying = true;
        // 互斥逻辑：关闭其他两个功能
        if (dashScript != null) dashScript.TurnOff();
        if (buildScript != null) buildScript.TurnOff();

        // 可以在这里显示一个鼠标图标，提示“现在是附魔模式”
    }

    public void TurnOff()
    {
        isModifying = false;
    }

    void HandleModification()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 发射一条射线去检测鼠标点到了什么
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, wallLayer);

            if (hit.collider != null)
            {
                GameObject targetWall = hit.collider.gameObject;

                // 检查这个墙是不是我们生成的长方形 (可以通过名字或者Tag判断)
                // 或者简单粗暴：只要有点到了，就加弹性！

                // 防止重复添加：如果没有 BouncySurface 组件，才添加
                if (targetWall.GetComponent<BouncySurface>() == null)
                {
                    targetWall.AddComponent<BouncySurface>();
                    // 添加成功后，为了防止误触，可以自动关闭工具，或者保持开启继续点下一个
                    // TurnOff(); 
                }
            }
        }
    }
}
