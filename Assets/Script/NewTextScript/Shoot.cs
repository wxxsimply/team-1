using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("拖入作为子物体的指针对象")]
    public GameObject arrowVisual;

    [Tooltip("施加力的大小")]
    public float dashForce = 15f;

    [Tooltip("是否在冲刺时让重力暂时失效？(手感通常更好)")]
    public bool resetVelocityOnDash = true;

    // 内部变量
    private Rigidbody2D rb;
    private bool isAiming = false; // 当前是否处于瞄准状态
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        // 游戏开始时隐藏指针
        if (arrowVisual != null)
        {
            arrowVisual.SetActive(false);
        }
    }

    void Update()
    {
        // 1. 检测按下数字键 '1'
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleAiming();
        }

        // 如果处于瞄准状态，执行旋转和发射逻辑
        if (isAiming)
        {
            RotatePointerFollowMouse();

            // 2. 检测鼠标左键点击
            if (Input.GetMouseButtonDown(0))
            {
                ApplyDashForce();
            }
        }
    }

    // 切换瞄准模式的开关
    void ToggleAiming()
    {
        isAiming = !isAiming;

        // 根据状态显示或隐藏指针
        if (arrowVisual != null)
        {
            arrowVisual.SetActive(isAiming);
        }
    }

    // 让指针跟随鼠标旋转的核心逻辑
    void RotatePointerFollowMouse()
    {
        // 获取鼠标在屏幕上的位置，并转换为世界坐标
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // 计算鼠标指向玩家的方向向量 (目标点 - 当前点)
        Vector2 direction = mousePos - transform.position;

        // 计算角度 (Atan2 返回的是弧度，需要转为度数)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 设置指针的旋转 (仅在Z轴旋转)
        arrowVisual.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 施加力的逻辑
    void ApplyDashForce()
    {
        // 既然指针已经旋转好了，我们可以直接利用指针的 "右方" 作为发射方向
        // 因为我们在 Unity 2D 中，物体的 transform.right 就是它旋转后的朝向
        Vector2 dashDirection = arrowVisual.transform.right;

        // 可选：为了手感更好，冲刺前先清空当前的惯性
        if (resetVelocityOnDash)
        {
            rb.velocity = Vector2.zero;
        }

        // 施加瞬间的力 (Impulse 模式适合瞬间爆发)
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        // 发射后关闭瞄准模式
        ToggleAiming();
    }
}
