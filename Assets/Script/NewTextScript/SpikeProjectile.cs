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

    void OnTriggerEnter2D(Collider2D other)
    {
        // ---------------------------------------------------------
        // 【新增修复】忽略发射器自己的触发范围
        // 如果撞到的东西也是个 Trigger (且不是玩家)，说明是空气/陷阱范围，直接无视
        if (other.isTrigger && !other.CompareTag("Player"))
        {
            return; // 直接结束，不执行下面的销毁代码
        }
        // ---------------------------------------------------------

        // 1. 撞人逻辑 (玩家身上的 Collider 通常不是 Trigger，或者如果是也没关系，上面排除了)
        if (other.CompareTag("Player"))
        {
            // 1. 检查是不是【真身】
            PlayerRespawn playerScript = other.GetComponent<PlayerRespawn>();
            if (playerScript != null)
            {
                playerScript.Die();
                Destroy(gameObject);
                return;
            }

            // 2. 检查是不是【镜像】(新增逻辑)
            MirrorLink mirrorScript = other.GetComponent<MirrorLink>();
            if (mirrorScript != null)
            {
                mirrorScript.Die(); // 调用中介的死亡方法
                Destroy(gameObject);
                return;
            }
        }

        // 2. 撞墙逻辑
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall") ||
                 other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                 other.gameObject.layer == 0) // Default 层
        {
            Destroy(gameObject);
        }
    }
}
