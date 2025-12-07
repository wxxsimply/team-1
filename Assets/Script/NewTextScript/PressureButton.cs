using System.Collections;
using System.Collections.Generic; // 引入列表
using UnityEngine;
using UnityEngine.Events; // 引入事件系统

public class PressureButton : MonoBehaviour
{
    [Header("视觉设置")]
    public Sprite buttonUp;   // 没按下的图片
    public Sprite buttonDown; // 按下的图片
    public SpriteRenderer sr; // 自身的渲染器

    [Header("功能设置")]
    // 这是一个可以在编辑器里配置的事件
    public UnityEvent onPressed;  // 按下时触发
    public UnityEvent onReleased; // (可选) 松开时触发

    // 内部变量：记录当前有几个东西压在按钮上
    private int objectsOnButton = 0;

    void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        // 初始状态
        UpdateButtonState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测 Player 或者 Projectile (尖刺)
        if (other.CompareTag("Player") || other.CompareTag("Projectile"))
        {
            objectsOnButton++; // 计数 +1

            // 如果是第一个压上来的东西，触发按下事件
            if (objectsOnButton == 1)
            {
                Press();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Projectile"))
        {
            objectsOnButton--; // 计数 -1

            // 防止计数器变成负数（虽然理论上不会）
            if (objectsOnButton < 0) objectsOnButton = 0;

            // 如果上面没东西了，触发松开事件
            if (objectsOnButton == 0)
            {
                Release();
            }
        }
    }

    void Press()
    {
        Debug.Log("按钮被按下！");
        if (buttonDown != null) sr.sprite = buttonDown; // 换图

        // 执行你在编辑器里绑定的所有功能
        onPressed.Invoke();
    }

    void Release()
    {
        Debug.Log("按钮松开！");
        if (buttonUp != null) sr.sprite = buttonUp; // 换图

        // 执行松开逻辑
        onReleased.Invoke();
    }

    //以此确保图片是对的
    void UpdateButtonState()
    {
        if (objectsOnButton > 0 && buttonDown != null) sr.sprite = buttonDown;
        else if (buttonUp != null) sr.sprite = buttonUp;
    }
}