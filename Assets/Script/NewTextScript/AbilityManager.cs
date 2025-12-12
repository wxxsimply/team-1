using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间

public class AbilityManager : MonoBehaviour
{
    [Header("子弹时间设置")]
    public float slowMotionScale = 0.3f;
    public bool isBulletTime = false;

    [Header("精力条设置")]
    public float maxStamina = 5.0f; // 最大持续时间 5秒
    public float currentStamina;    // 当前剩余时间
    [Tooltip("如果不想要UI，这里可以留空")]
    public Image staminaBarImage;   // 拖入 UI 图片 (Fill模式) 来显示进度

    [Header("地面检测设置")]
    public Transform groundCheck;   // 拖入脚底的空物体
    public float groundCheckRadius = 0.2f; // 检测半径
    public LayerMask groundLayer;   // 选择 Ground 层
    private bool isGrounded;        // 内部变量：是否在地上

    [Header("引用各个能力脚本")]
    public PlayerDirectionalDash dashScript;
    public BlockBuilder buildScript;
    public ModifierTool modifierScript;
    public DebugDestroyer debugScript;
    public RandomAppearance appearanceScript;

    private int activeAbilityIndex = -1;

    void Start()
    {
        // 游戏开始，精力全满
        currentStamina = maxStamina;
    }

    void Update()
    {
        // 1. 先检测是否在地上
        CheckGround();

        // 2. 处理精力的消耗和回复
        HandleStamina();

        // 3. 处理子弹时间开关
        HandleBulletTime();

        if (isBulletTime)
        {
            HandleAbilityInput();

            // 鼠标左键确认变身
            if (activeAbilityIndex != -1 && Input.GetMouseButtonDown(0))
            {
                TriggerAppearanceChange();
            }
        }
        else
        {
            CloseAllAbilities();
        }

        // 4. 更新 UI 显示
        UpdateUI();
    }

    // --- 物理检测 ---
    void CheckGround()
    {
        if (groundCheck != null)
        {
            // 在脚底画一个小圆圈，看看有没有碰到 Ground 层
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    // --- 精力逻辑 ---
    void HandleStamina()
    {
        if (isGrounded)
        {
            // 只要在地上，精力瞬间回满
            currentStamina = maxStamina;
        }
        else if (isBulletTime)
        {
            // 如果在空中 且 开启了子弹时间 -> 消耗精力
            // 注意使用 unscaledDeltaTime (真实时间)，不受慢动作影响
            currentStamina -= Time.unscaledDeltaTime;

            // 限制不要跌破 0
            if (currentStamina < 0) currentStamina = 0;
        }
    }

    // --- 子弹时间逻辑 ---
    void HandleBulletTime()
    {
        // 开启条件：
        // 1. 按住 LeftShift
        // 2. 并且 (精力大于0 或者 就在地上)
        //    (如果在地上，就算精力扣了一点也会瞬间补满，所以相当于无限)

        bool inputPressed = Input.GetKey(KeyCode.LeftShift);
        bool hasStamina = currentStamina > 0;

        if (inputPressed && hasStamina)
        {
            isBulletTime = true;
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            // 如果松开了键，或者精力耗尽了
            isBulletTime = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    void UpdateUI()
    {
        if (staminaBarImage != null)
        {
            // FillAmount 范围是 0 到 1
            staminaBarImage.fillAmount = currentStamina / maxStamina;

            // 可选：精力满的时候隐藏 UI，不满的时候显示
            // staminaBarImage.gameObject.SetActive(currentStamina < maxStamina);
        }
    }

    // --- 下面是之前的代码，保持不变 ---

    void HandleAbilityInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateAbility(2);
        }
        // --- 修改点：这里检测 3 键，用来激活删除 ---
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateAbility(3); // 传入索引 3
        }
        // 之前的弹性附魔已经移到 4 了
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActivateAbility(4);
        }
    }

    void ActivateAbility(int index)
    {
        CloseAllAbilities();
        activeAbilityIndex = index;

        switch (index)
        {
            case 1:
                if (dashScript != null) dashScript.TurnOn();
                break;
            case 2:
                if (buildScript != null) buildScript.TurnOn();
                break;

            // --- 修改点：把 case 0 改成 case 3 ---
            case 3:
                // 现在 3 号对应的是“删除工具”(DebugDestroyer)
                if (debugScript != null) debugScript.TurnOn();
                break;

            case 4:
                if (modifierScript != null) modifierScript.TurnOn();
                break;
        }
    }

    void CloseAllAbilities()
    {
        if (dashScript != null) dashScript.TurnOff();
        if (buildScript != null) buildScript.TurnOff();
        if (modifierScript != null) modifierScript.TurnOff();
        if (debugScript != null) debugScript.TurnOff();

        activeAbilityIndex = -1;
    }

    void TriggerAppearanceChange()
    {
        if (appearanceScript != null)
        {
            appearanceScript.ChangeSpriteRandomly();
        }
    }

    // 为了方便调试，在编辑器里画出检测圈
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
