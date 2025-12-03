using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractableLamp : MonoBehaviour
{
    [Header("灯光设置")]
    public Light2D lampLight;
    public bool isLit = false;

    void Start()
    {
        // 确保一开始状态正确
        if (lampLight != null)
            lampLight.enabled = isLit;
    }

    // 当玩家进入触发区域
    void OnTriggerEnter2D(Collider2D other)
    {
        // 假设主角的 Tag 是 "Player"
        if (other.CompareTag("Player") && !isLit)
        {
            // 这里可以做一个简单的自动点亮，或者按下 E 键点亮
            // 为了原型开发快速验证，我们先写成“走过去自动点亮”
            LightUp();
        }
    }

    // 如果想做按键交互，可以用 OnTriggerStay2D 配合 Input 判断
    /*
    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isLit) {
            LightUp();
        }
    }
    */

    void LightUp()
    {
        isLit = true;
        lampLight.enabled = true;
        Debug.Log("灯亮了！秩序恢复了一点点...");
    }
}
