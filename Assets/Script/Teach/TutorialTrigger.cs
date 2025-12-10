using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 必须引入 TextMeshPro 的命名空间

public class TutorialTrigger : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("这里写你想对玩家说的话")]
    [TextArea(3, 10)] // 让输入框变大，方便写多行文字
    public string tutorialMessage;

    [Tooltip("把 Canvas 下面的那个 TutorialText 拖进来")]
    public TextMeshProUGUI uiTextComponent; // 注意类型是 TextMeshProUGUI

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. 把这一关的文字填进去
            if (uiTextComponent != null)
            {
                uiTextComponent.text = tutorialMessage;

                // 2. 显示文字
                uiTextComponent.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 离开区域后，隐藏文字
            if (uiTextComponent != null)
            {
                uiTextComponent.gameObject.SetActive(false);
            }
        }
    }
}
