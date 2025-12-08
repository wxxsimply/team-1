using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("子弹时间设置")]
    public float slowMotionScale = 0.3f; // 慢动作时的倍速 (0.3 表示 30% 速度)
    public bool isBulletTime = false;    // 当前是否处于子弹时间

    [Header("引用各个能力脚本")]
    public PlayerDirectionalDash dashScript;    // 1号能力
    public BlockBuilder buildScript;            // 2号能力
    public ModifierTool modifierScript;         // 3号能力
    public DebugDestroyer debugScript;          // 0号调试工具 (通常不受限制，也可加上)

    [Header("引用外观脚本")]
    public RandomAppearance appearanceScript;   // 变身脚本

    // 记录当前激活的是哪个能力，方便取消
    private int activeAbilityIndex = -1; // -1表示没激活，1=冲刺, 2=建造, 3=附魔

    void Update()
    {
        HandleBulletTime();

        // 只有在子弹时间（或者已经激活了某种能力）的情况下，才允许切换能力
        // 这里设计为：必须按住 E 或者 开启了 E 模式才能选能力
        if (isBulletTime)
        {
            HandleAbilityInput();
        }
        else
        {
            // 如果退出了子弹时间，是否要强制关闭所有能力？
            // 通常为了手感，如果玩家正在瞄准（比如按了2正在旋转方块），突然松开E
            // 建议：要么保持当前能力，要么强制关闭。
            // 这里我们设定：松开E（退出子弹时间）会强制关闭所有瞄准状态，恢复正常游戏。
            CloseAllAbilities();
        }
    }

    void HandleBulletTime()
    {
        // --- 修改点：现在使用左 Shift 键 (LeftShift) ---
        // 按住 Shift 进入子弹时间，松开恢复
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isBulletTime = true;
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            isBulletTime = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    void HandleAbilityInput()
    {
        // 监听 1, 2, 3 (以及 0)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateAbility(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateAbility(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // 0号工具通常作为调试，随时可以用，或者也限制在子弹时间
            ActivateAbility(0);
        }
    }

    void ActivateAbility(int index)
    {
        // 1. 先关闭所有正在运行的能力 (互斥逻辑)
        CloseAllAbilities();

        // 2. 根据按键开启对应的能力
        activeAbilityIndex = index;

        switch (index)
        {
            case 1:
                if (dashScript != null) dashScript.TurnOn();
                TriggerAppearanceChange(); // 触发变身
                break;
            case 2:
                if (buildScript != null) buildScript.TurnOn();
                TriggerAppearanceChange(); // 触发变身
                break;
            case 3:
                if (modifierScript != null) modifierScript.TurnOn();
                TriggerAppearanceChange(); // 触发变身
                break;
            case 0:
                if (debugScript != null) debugScript.TurnOn();
                // 0号通常不需要变身，或者你想变也可以加
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
        // 调用之前的变身脚本
        if (appearanceScript != null)
        {
            appearanceScript.ChangeSpriteRandomly(); // 需要把这个方法改为 public
        }
    }
}
