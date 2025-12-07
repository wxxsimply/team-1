using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionalDash : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("请把挂着 BlockBuilder 脚本的物体拖进来")]
    public BlockBuilder blockBuilderScript; // 新增：引用建造脚本
    public ModifierTool modifierScript;
    public DebugDestroyer debugScript;

    [Header("设置")]
    public GameObject arrowVisual;
    public float dashForce = 15f;
    public bool resetVelocityOnDash = true;

    private Rigidbody2D rb;
    private bool isAiming = false;
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        TurnOff(); // 游戏开始时确保关闭
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 如果原本是关的，就打开；原本是开的，就关闭
            if (!isAiming)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }

        if (isAiming)
        {
            RotatePointerFollowMouse();
            if (Input.GetMouseButtonDown(0))
            {
                ApplyDashForce();
            }
        }
    }

    // 新增：专门用来开启的方法
    public void TurnOn()
    {
        isAiming = true;
        if (arrowVisual != null) arrowVisual.SetActive(true);
        if (debugScript != null) debugScript.TurnOff();// 开启冲刺时，强制关闭删除模式

        // 关键点：开启冲刺时，强制关闭建造模式！
        if (blockBuilderScript != null) blockBuilderScript.TurnOff();
        if (modifierScript != null) modifierScript.TurnOff();
    }

    // 新增：专门用来关闭的方法 (public 为了让别的脚本也能调用)
    public void TurnOff()
    {
        isAiming = false;
        if (arrowVisual != null) arrowVisual.SetActive(false);
    }

    void RotatePointerFollowMouse()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowVisual.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ApplyDashForce()
    {
        Vector2 dashDirection = arrowVisual.transform.right;
        if (resetVelocityOnDash) rb.velocity = Vector2.zero;
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        TurnOff(); // 发射后关闭
    }
}