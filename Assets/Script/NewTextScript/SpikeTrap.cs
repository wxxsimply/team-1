using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("拖入做好的尖刺 Prefab")]
    public GameObject spikePrefab;

    [Tooltip("拖入子物体 FirePoint")]
    public Transform firePoint;

    public float spikeSpeed = 15f; // 尖刺飞行速度
    public float cooldown = 1.5f;  // 发射冷却时间

    private float nextFireTime = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只有玩家进入，并且冷却时间结束了，才发射
        if (other.CompareTag("Player") && Time.time >= nextFireTime)
        {
            FireSpike();
            // 设置下一次允许发射的时间
            nextFireTime = Time.time + cooldown;
        }
    }

    void FireSpike()
    {
        if (spikePrefab == null || firePoint == null) return;

        // 1. 在发射点生成尖刺，角度跟随发射点
        GameObject newSpike = Instantiate(spikePrefab, firePoint.position, firePoint.rotation);

        // 2. 获取尖刺的刚体，给它一个速度
        Rigidbody2D rb = newSpike.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 朝着发射点的“右方”发射
            rb.velocity = firePoint.right * spikeSpeed;
        }
    }
}
