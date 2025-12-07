using UnityEngine;

public class DebugDestroyer : MonoBehaviour
{
    [Header("引用其他工具")]
    // 引用其他3个脚本，在开启删除模式时把它们关掉
    public PlayerDirectionalDash dashScript;
    public BlockBuilder buildScript;
    public ModifierTool modifierScript;

    [Header("设置")]
    [Tooltip("确保只删除墙体，防止误删玩家或背景")]
    public LayerMask targetLayer;

    private bool isDeleting = false;

    void Start()
    {
        TurnOff();
    }

    void Update()
    {
        // 按 0 键切换删除模式
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (!isDeleting) TurnOn();
            else TurnOff();
        }

        if (isDeleting)
        {
            HandleDeletion();
        }
    }

    public void TurnOn()
    {
        isDeleting = true;

        // 互斥逻辑：强制关闭其他所有功能
        if (dashScript != null) dashScript.TurnOff();
        if (buildScript != null) buildScript.TurnOff();
        if (modifierScript != null) modifierScript.TurnOff();

        Debug.Log("进入删除模式：点击墙体进行销毁");
    }

    public void TurnOff()
    {
        isDeleting = false;
    }

    void HandleDeletion()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 发射射线检测鼠标位置的物体
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, targetLayer);

            if (hit.collider != null)
            {
                // 获取被打中的物体
                GameObject targetObj = hit.collider.gameObject;

                // --- 安全销毁 ---
                // Destroy 会在这一帧结束时，把物体连同它身上的 BouncySurface 脚本、
                // 碰撞体、渲染器全部抹除。
                // 就算它刚才正好把你弹飞，现在销毁也不会报错。
                Destroy(targetObj);
            }
        }
    }
}
