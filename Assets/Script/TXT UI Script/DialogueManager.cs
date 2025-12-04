using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引用 TextMeshPro
using System.Collections; // 必须引用这个，用于协程[

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI 组件")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("打字机设置")]
    [Tooltip("每个字显示的时间间隔（越小越快）")]
    public float typingSpeed = 0.05f;

    private Coroutine typingCoroutine; // 用来记录当前正在打字的进程

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    // 显示对话（外部调用这个）
    public void ShowDialogue(string content)
    {
        dialoguePanel.SetActive(true);

        // 如果之前还有没打完的字，先停掉，防止字重叠乱跳
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // 开启新的打字协程
        typingCoroutine = StartCoroutine(TypeText(content));
    }

    // 关闭对话
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        // 关闭时也记得停止打字，防止后台还在跑
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
    }

    // 核心逻辑：打字机协程
    IEnumerator TypeText(string content)
    {
        // 1. 先清空文本
        dialogueText.text = "";

        // 2. 逐个字符遍历
        foreach (char letter in content.ToCharArray())
        {
            // 把当前字符加到文本框里
            dialogueText.text += letter;

            // 随机等待 0.02秒 到 0.1秒 之间
            float randomSpeed = Random.Range(0.02f, 0.1f);
            yield return new WaitForSeconds(randomSpeed);
        }

        // 打字结束，清空记录
        typingCoroutine = null;
    }
}
