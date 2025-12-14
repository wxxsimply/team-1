using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 如果需要控制文字颜色等
using UnityEngine.SceneManagement; // 如果是切换场景

public class DoorInteraction : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("把刚才做的 DoorPromptCanvas 拖进来")]
    public GameObject promptUI;

    [Tooltip("下一关的名字")]
    public string nextSceneName;

    private bool isPlayerInRange = false;

    void Start()
    {
        // 游戏开始时，先把提示藏起来
        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        // 只有玩家在范围内，且按下了 E 键
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            EnterDoor();
        }
    }

    void EnterDoor()
    {
        Debug.Log("进入门！");
        // 这里写你的传送逻辑，比如：
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // --- 触发检测 ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // 玩家靠近，显示提示
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // 玩家离开，隐藏提示
            if (promptUI != null) promptUI.SetActive(false);
        }
    }
}