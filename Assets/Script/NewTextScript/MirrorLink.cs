using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorLink : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("请把【真身 Player】拖进来")]
    public PlayerRespawn mainPlayerScript;

    // --- 1. 处理发射尖刺 (被动被杀) ---
    // 供外部（如 SpikeProjectile）调用
    public void Die()
    {
        if (mainPlayerScript != null)
        {
            Debug.Log("镜像被杀，真身陪葬！");
            mainPlayerScript.Die();
        }
    }

    // --- 2. 处理固定尖刺 (主动撞击) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果镜像撞墙，主角也死（这会让游戏变得非常难，慎用！）
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Die(); 
        }
        // 假设你的固定尖刺 Tag 是 "Projectile" 或者 Layer 是 "Trap"
        // 你需要根据你的实际情况修改这里的判断条件
        if (other.CompareTag("Projectile") || other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            Die();
        }
    }
}