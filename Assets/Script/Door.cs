using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用这个命名空间才能切场景

public class SceneDoor : MonoBehaviour
{
    [Header("场景设置")]
    [Tooltip("要加载的场景名称（必须完全一致）")]
    public string sceneToLoad;

    [Header("交互设置")]
    public KeyCode interactKey = KeyCode.E;

    [Header("视觉提示（可选）")]
    public GameObject hintUI; // 比如一个“按E”的小图标

    private bool canEnter = false;

    void Start()
    {
        // 游戏开始时隐藏提示
        if (hintUI != null) hintUI.SetActive(false);
    }

    void Update()
    {
        // 只有当玩家在门口 且 按下按键
        if (canEnter && Input.GetKeyDown(interactKey))
        {
            SwitchScene();
        }
    }

    void SwitchScene()
    {
        // 检查名字是否为空，防止报错
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("请在 Inspector 里填入要加载的场景名字！");
            return;
        }

        // 加载场景
        SceneManager.LoadScene(sceneToLoad);
    }

    // 玩家进入感应区
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = true;
            if (hintUI != null) hintUI.SetActive(true);
        }
    }

    // 玩家离开感应区
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = false;
            if (hintUI != null) hintUI.SetActive(false);
        }
    }
}
