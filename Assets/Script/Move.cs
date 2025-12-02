using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 记得给你的角色添加 Rigidbody2D 组件，并将 Gravity Scale 设置合适的值
// 为了防止角色在斜坡下滑，可以将 Rigidbody2D 的 Collision Detection 设为 Continuous
[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [Header("移动参数")]
    [Tooltip("移动速度")]
    public float moveSpeed = 8f;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isFacingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. 获取输入
        // 使用 GetAxisRaw 而不是 GetAxis，可以获得 -1, 0, 1 的瞬时值
        // 从而实现“按下立刻走，松开立刻停”的硬手感
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. 处理图像翻转
        // 只有当有输入且方向与当前朝向不一致时才翻转
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        // 3. 执行物理移动
        // 保留原有的 Y 轴速度（如重力下落），只修改 X 轴
        // 直接修改 velocity 比 AddForce 更适合解密游戏的精确控制
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    // 翻转逻辑
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f; // 将 X 轴缩放取反
        transform.localScale = localScale;
    }
}
