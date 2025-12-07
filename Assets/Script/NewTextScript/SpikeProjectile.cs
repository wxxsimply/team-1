using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeProjectile : MonoBehaviour
{
    [Header("设置")]
    public float damage = 1f; // 伤害值（留着以后用）
    public float lifeTime = 3f; // 3秒后自动销毁，防止无限飞

    void Start()
    {
        // 发射后若干秒自动销毁，节省性能
        Destroy(gameObject, lifeTime);
    }

    // 当发生实体碰撞时调用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果撞到了墙 (假设你的墙体Layer叫 "Wall" 或 "Ground")
        // 这里我们简单判断：只要不是撞到玩家，就销毁自己
        // (因为我们希望它撞到玩家时造成伤害，撞到墙时销毁)

        if (collision.gameObject.CompareTag("Player"))
        {
            // --- 这里写对玩家造成伤害的逻辑 ---
            Debug.Log("玩家中刺！");
            // 比如： collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

            // 撞到人后销毁尖刺
            Destroy(gameObject);
        }
        else
        {
            // 撞到了墙、地面等其他障碍物
            // 播放一个撞击音效或火花特效可以在这里加
            Destroy(gameObject);
        }
    }
}
