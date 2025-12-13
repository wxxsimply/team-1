using UnityEngine;

public class DebugDestroyer : MonoBehaviour
{
    [Header("引用")]
    public BlockBuilder buildScript; // 必须引用建造脚本！

    // (这些引用是为了互斥，保持不变)
    public PlayerDirectionalDash dashScript;
    public ModifierTool modifierScript;

    private bool isDeleting = false;

    void Update()
    {
        if (isDeleting)
        {
            HandleRemoteDetonation();
        }
    }

    public void TurnOn()
    {
        isDeleting = true;
        // 关闭其他能力
        if (dashScript != null) dashScript.TurnOff();
        if (buildScript != null) buildScript.TurnOff();
        if (modifierScript != null) modifierScript.TurnOff();

        Debug.Log("删除模式就绪：点击左键直接销毁墙体");
    }

    public void TurnOff()
    {
        isDeleting = false;
    }

    // --- 修改点：不再瞄准，直接按左键消除 ---
    void HandleRemoteDetonation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (buildScript != null)
            {
                // 直接调用刚才在 BlockBuilder 里写的那个方法
                buildScript.DestroyCurrentWall();

                // (可选) 炸完之后自动退出删除模式？如果你想删完自动切回普通状态，把下面这行注释取消
                // AbilityManager.Instance.CloseAllAbilities(); // 需要你自己写个单例或者在 Manager 里处理，这里先不加
            }
        }
    }
}