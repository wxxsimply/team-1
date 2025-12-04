using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [Header("设置")]
    [TextArea] // 让输入框变大，方便写多行字
    public string message = "这是一段默认信息...";

    public KeyCode interactKey = KeyCode.E;

    [Header("提示UI（可选）")]
    public GameObject hintIcon; // 比如头顶的“E”图标

    private bool canInteract = false;
    private bool isTalking = false; // 记录当前是否正在显示

    void Update()
    {
        if (canInteract && Input.GetKeyDown(interactKey))
        {
            if (!isTalking)
            {
                // 打开对话
                DialogueManager.instance.ShowDialogue(message);
                isTalking = true;
            }
            else
            {
                // 再次按键，关闭对话
                DialogueManager.instance.HideDialogue();
                isTalking = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if (hintIcon != null) hintIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            isTalking = false; // 重置状态
            if (hintIcon != null) hintIcon.SetActive(false);

            // 离开时强制关闭对话框，防止走远了字还飘在屏幕上
            DialogueManager.instance.HideDialogue();
        }
    }
}
