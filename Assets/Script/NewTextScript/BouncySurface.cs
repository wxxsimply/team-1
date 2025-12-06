using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncySurface : MonoBehaviour
{
    [Header("反弹设置")]
    public float bounceForce = 15f; // 基础反弹力度
    public Color bouncyColor = Color.cyan; // 变成什么颜色代表有弹性？

    private SpriteRenderer sr;

    void Start()
    {
        // 1. 变色：让玩家知道这个墙现在有弹性了
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = bouncyColor;
        }
    }

    // 2. 核心物理逻辑：当发生碰撞时
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查撞上来的是不是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // 获取碰撞点的信息
                ContactPoint2D contact = collision.contacts[0];

                // --- 数学魔法时间 ---
                // 获取玩家当前的速度（入射向量）
                // 注意：由于碰撞瞬间速度可能已经衰减，有时候用相对速度更准，
                // 但为了简单爽快，我们直接取一个带有力度的方向。

                // 我们使用 collision.relativeVelocity 来获取撞击瞬间的猛烈程度
                Vector2 incomingVelocity = collision.relativeVelocity;

                // contact.normal 就是墙体的“垂直方向”（法线）
                // Vector2.Reflect 会自动计算“入射角=反射角”，也就是法线作为角平分线
                Vector2 reflectDir = Vector2.Reflect(incomingVelocity, contact.normal).normalized;

                // 施加反弹力
                // 瞬间改变速度 (Impulse)
                playerRb.velocity = Vector2.zero; // 先清空原有速度防止干扰
                playerRb.AddForce(reflectDir * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
