using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("子弹时间设置")]
    public float slowMotionScale = 0.3f;
    public bool isBulletTime = false;

    [Header("引用各个能力脚本")]
    public PlayerDirectionalDash dashScript;
    public BlockBuilder buildScript;
    public ModifierTool modifierScript;
    public DebugDestroyer debugScript;

    [Header("引用外观脚本")]
    public RandomAppearance appearanceScript;

    // 记录当前激活的是哪个能力 (-1表示没激活)
    private int activeAbilityIndex = -1;

    void Update()
    {
        HandleBulletTime();

        if (isBulletTime)
        {
            HandleAbilityInput();

            // --- 新增：在这里检测鼠标点击 ---
            // 只有在选了能力的情况下，按下鼠标左键，才变身
            if (activeAbilityIndex != -1 && Input.GetMouseButtonDown(0))
            {
                TriggerAppearanceChange();
            }
        }
        else
        {
            CloseAllAbilities();
        }
    }

    void HandleBulletTime()
    {
        // 按住 LeftShift 进入子弹时间
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) ActivateAbility(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ActivateAbility(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ActivateAbility(3);
        else if (Input.GetKeyDown(KeyCode.Alpha0)) ActivateAbility(0);
    }

    void ActivateAbility(int index)
    {
        CloseAllAbilities();
        activeAbilityIndex = index;

        // --- 修改点：这里只负责开启瞄准/预览，不再变身 ---
        switch (index)
        {
            case 1:
                if (dashScript != null) dashScript.TurnOn();
                // TriggerAppearanceChange(); // <--- 这一行被删掉了
                break;
            case 2:
                if (buildScript != null) buildScript.TurnOn();
                // TriggerAppearanceChange(); // <--- 这一行被删掉了
                break;
            case 3:
                if (modifierScript != null) modifierScript.TurnOn();
                // TriggerAppearanceChange(); // <--- 这一行被删掉了
                break;
            case 0:
                if (debugScript != null) debugScript.TurnOn();
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
}
