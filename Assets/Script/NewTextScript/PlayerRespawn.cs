using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 如果你想重置整个关卡，需要这个

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint; // 记录出生点坐标
    private Rigidbody2D rb;

    void Start()
    {
        // 1. 游戏刚开始时，记住现在站在哪里
        spawnPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();
    }

    // --- 供外界调用的死亡方法 ---
    public void Die()
    {
        Debug.Log("玩家死亡，正在重生...");

        // 方法 A：直接把玩家瞬移回出生点 (推荐，速度快)
        transform.position = spawnPoint;

        // 重要：重生时要把速度归零！
        // 否则如果你是冲刺撞死的，重生后还会继续带着速度飞出去
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // 方法 B：重载当前场景 (如果你希望墙壁、怪物的状态也全部重置)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // (进阶) 如果你有存档点，可以用这个方法更新出生点
    public void SetNewSpawnPoint(Vector3 newPos)
    {
        spawnPoint = newPos;
    }
}
