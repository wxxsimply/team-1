using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("拖入你要传送到的目标位置（另一个门）")]
    public Transform destination;

    [Tooltip("防止无限循环传送的冷却时间")]
    public float cooldownTime = 0.5f;

    // 这是一个“门锁”，每个门都有自己的锁，互不影响
    private bool isLocked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果门被锁了，谁都别想进
        if (isLocked) return;

        // --- 核心修改：同时检测 "Player" 和 "Projectile" ---
        if (other.CompareTag("Player") || other.CompareTag("Projectile"))
        {
            if (destination != null)
            {
                StartCoroutine(TeleportRoutine(other));
            }
        }
    }

    IEnumerator TeleportRoutine(Collider2D traveler)
    {
        // 1. 传送！
        // 获取物体的 Transform
        Transform targetTransform = traveler.transform;

        // 移动位置
        targetTransform.position = destination.position;

        // --- 进阶优化：通知对面的门暂时关闭 ---
        // 这一步是为了防止你传过去瞬间，对面的门又把你传回来（死循环）
        // 我们尝试在 destination 上找 Portal 脚本
        Portal destPortal = destination.GetComponent<Portal>();
        if (destPortal != null)
        {
            // 告诉对面的门：“有人要过来了，你先闭嘴 0.5 秒”
            destPortal.StartCoroutine(destPortal.LockPortalTemporarily());
        }

        // 2. 锁定自己一小会儿 (防止重复触发)
        yield return StartCoroutine(LockPortalTemporarily());
    }

    // 一个专门用来锁门的协程
    public IEnumerator LockPortalTemporarily()
    {
        isLocked = true;
        yield return new WaitForSeconds(cooldownTime);
        isLocked = false;
    }
}
