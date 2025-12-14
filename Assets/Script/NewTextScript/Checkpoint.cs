using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("被激活后变成什么颜色？(例如绿色)")]
    public Color activeColor = Color.green;

    private bool isActivated = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果已经被激活过，就不用再处理了
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            // 1. 找到玩家身上的复活脚本
            PlayerRespawn respawnScript = other.GetComponent<PlayerRespawn>();

            if (respawnScript != null)
            {
                // 2. 更新复活点坐标为当前旗帜的位置
                respawnScript.SetNewSpawnPoint(transform.position);

                // 3. 标记为已激活
                ActivateCheckpoint();
            }
        }
    }

    void ActivateCheckpoint()
    {
        isActivated = true;
        Debug.Log("存档点已激活！");

        // 视觉反馈：变色
        if (sr != null)
        {
            sr.color = activeColor;
        }

        // (进阶) 这里可以播放音效或动画
    }
}