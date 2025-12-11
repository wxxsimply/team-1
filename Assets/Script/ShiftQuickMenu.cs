using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleShiftUI : MonoBehaviour
{
    [Header("UI引用")]
    public GameObject uiPanel;

    [Header("按钮引用")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;

    [Header("缩放设置")]
    [SerializeField] 
    public float scaleDownAmountx = 0.002f; // 每次缩小的固定数值
    public float scaleDownAmounty = 0.2f;

    private Dictionary<Button, Vector3> initialScales = new Dictionary<Button, Vector3>();
    private Button currentPressedButton = null;

    void Start()
    {
        uiPanel.SetActive(false); // 初始隐藏UI

        // 保存所有按钮的初始缩放值
        Button[] buttons = { button1, button2, button3, button4, button5 };
        foreach (var button in buttons)
        {
            if (button != null)
            {
                initialScales[button] = button.transform.localScale;
            }
        }
    }

    void Update()
    {
        // Shift控制UI显示/隐藏
        if (Input.GetKey(KeyCode.LeftShift))
        {
            uiPanel.SetActive(true);
            CheckNumberInput();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            uiPanel.SetActive(false);
            ResetAllButtons(); // 隐藏UI时重置所有按钮
        }
    }

    void CheckNumberInput()
    {
        // 处理数字键1-5的按下
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetCurrentButton();
            currentPressedButton = button1;
            ScaleButtonDown(button1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetCurrentButton();
            currentPressedButton = button2;
            ScaleButtonDown(button2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetCurrentButton();
            currentPressedButton = button3;
            ScaleButtonDown(button3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResetCurrentButton();
            currentPressedButton = button4;
            ScaleButtonDown(button4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ResetCurrentButton();
            currentPressedButton = button5;
            ScaleButtonDown(button5);
        }
        // 检测其他数字键6-0按下时重置当前按钮
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Alpha7) ||
                 Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Alpha9) ||
                 Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetCurrentButton();
            currentPressedButton = null;
        }
    }

    // 缩放指定按钮
    void ScaleButtonDown(Button button)
    {
        if (button != null && initialScales.ContainsKey(button))
        {
            Vector3 initialScale = initialScales[button];
            Vector3 newScale = new Vector3(
                Mathf.Max(initialScale.x - scaleDownAmountx, 0.1f), // 最小缩放限制
                Mathf.Max(initialScale.y - scaleDownAmounty, 0.1f),
                initialScale.z
            );
            button.transform.localScale = newScale;
        }
    }

    // 重置当前被按下的按钮
    void ResetCurrentButton()
    {
        if (currentPressedButton != null && initialScales.ContainsKey(currentPressedButton))
        {
            currentPressedButton.transform.localScale = initialScales[currentPressedButton];
        }
    }

    // 重置所有按钮到初始状态
    void ResetAllButtons()
    {
        ResetCurrentButton();
        currentPressedButton = null;
    }
}