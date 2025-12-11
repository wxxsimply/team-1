using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用这个命名空间！

public class ScenePortal : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("下一关的场景名字（必须和 Build Settings 里的一模一样）")]
    public string nextSceneName = "Level_2";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只允许玩家触发
        if (other.CompareTag("Player"))
        {
            Debug.Log("正在前往下一关...");

            // 加载目标场景
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
