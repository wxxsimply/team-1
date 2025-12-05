using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("拖入你要传送到的目标位置（另一个门）")]
    public Transform destination;

    [Tooltip("防止无限循环传送的冷却时间")]
    public float cooldownTime = 1.0f;

    // 一个静态变量，用来记录玩家是否“刚刚传送过”
    // static 意味着所有传送门共享这个状态
    private static bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 只有玩家才能触发
        // 2. 并且当前没有在冷却时间内
        if (other.CompareTag("Player") && !isTeleporting)
        {
            if (destination != null)
            {
                StartCoroutine(TeleportRoutine(other.transform));
            }
            else
            {
                Debug.LogWarning("你忘了把目标传送门拖进 Destination 槽里！");
            }
        }
    }

    IEnumerator TeleportRoutine(Transform playerTransform)
    {
        // 锁定状态：告诉所有门，现在正在传送中，不要再次触发！
        isTeleporting = true;

        // --- 可以在这里播放传送音效或特效 ---

        // 执行传送：直接修改玩家位置
        playerTransform.position = destination.position;

        // 等待一段时间（冷却）
        // 这样玩家在到达B门时，不会立刻触发B门的传送
        yield return new WaitForSeconds(cooldownTime);

        // 解锁状态：冷却结束，允许下次传送
        isTeleporting = false;
    }
}
