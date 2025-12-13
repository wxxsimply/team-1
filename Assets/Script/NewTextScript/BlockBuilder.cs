using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("请把挂着 PlayerDirectionalDash 脚本的物体拖进来")]
    public PlayerDirectionalDash dashScript;

    // (如果你有 ModifierTool 的引用也可以放在这，没有就不管)
    public ModifierTool modifierScript;
    public DebugDestroyer debugScript;

    [Header("设置")]
    public GameObject previewObject;
    public GameObject blockPrefab;
    public float rotateSpeed = 100f;

    private bool isBuilding = false;

    // --- 修改点 1：新增一个变量，用来“记住”当前造出来的那个墙 ---
    private GameObject currentWall;

    void Start()
    {
        TurnOff();
    }

    void Update()
    {
        // 注意：现在按键切换由 AbilityManager 接管了
        // 这里我们只负责执行具体的建造逻辑
        if (isBuilding && previewObject != null)
        {
            FollowMouse();
            HandleRotation();
            HandleBuilding();

            // --- 视觉优化 (可选) ---
            // 如果墙已经存在，我们可以把预览图变成红色，或者隐藏，提示玩家不能造
            // 这里简单处理：如果墙存在，就把预览图隐藏，直观告诉玩家“不能造”
            if (currentWall != null)
            {
                previewObject.SetActive(false);
            }
            else
            {
                // 如果墙没了（被消除了），预览图就显示出来
                previewObject.SetActive(true);
            }
        }
    }

    public void TurnOn()
    {
        isBuilding = true;
        // 开启时，只有当没有墙的时候，才显示预览
        if (previewObject != null)
        {
            previewObject.SetActive(currentWall == null);
        }

        if (dashScript != null) dashScript.TurnOff();
        if (modifierScript != null) modifierScript.TurnOff();
        if (debugScript != null) debugScript.TurnOff();
    }

    public void TurnOff()
    {
        isBuilding = false;
        if (previewObject != null) previewObject.SetActive(false);
    }

    void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        previewObject.transform.position = mousePos;
    }

    void HandleRotation()
    {
        float rotationAmount = 0f;
        if (Input.GetKey(KeyCode.A)) rotationAmount = 1f;
        else if (Input.GetKey(KeyCode.D)) rotationAmount = -1f;

        // 只有预览图显示的时候才旋转，节省性能
        if (previewObject.activeSelf)
        {
            previewObject.transform.Rotate(0, 0, rotationAmount * rotateSpeed * Time.deltaTime);
        }
    }

    void HandleBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // --- 修改点 2：核心检查逻辑 ---

            // 检查：如果 currentWall 还有东西（即不为 null），说明场上已经有一个墙了
            if (currentWall != null)
            {
                Debug.Log("限制：场上只能存在一个由能力生成的墙体！请先按 0 消除。");
                // 直接返回，不执行后面的生成代码
                return;
            }

            // 如果代码跑到这里，说明 currentWall 是空的（要么没造过，要么被删了）
            // 生成墙体，并把它赋值给 currentWall 记住它
            currentWall = Instantiate(blockPrefab, previewObject.transform.position, previewObject.transform.rotation);

            // 生成完后关闭自己
            TurnOff();
        }
    }

    // ... 之前的代码 ...

    // --- 新增：供外部调用的“远程销毁”方法 ---
    public void DestroyCurrentWall()
    {
        if (currentWall != null)
        {
            Destroy(currentWall);
            currentWall = null; // 记得把变量清空，这样你才能造下一个
            Debug.Log("墙体已远程回收！");
        }
        else
        {
            Debug.Log("当前没有墙体可消除");
        }
    }
}
