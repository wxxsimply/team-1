using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("设置")]
    public float smoothSpeed = 5f; // 相机移动的速度（越大越快）

    // 内部变量：相机当前想要去的地方
    private Vector3 targetPosition;

    void Start()
    {
        // 游戏开始时，目标就是相机当前所在的位置
        targetPosition = transform.position;
    }

    void Update()
    {
        // --- 平滑移动逻辑 ---
        // 使用 Lerp (线性插值) 让相机缓缓移动到目标点
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 保持 Z 轴为 -10 (这是 2D 游戏的标准，否则背景会消失)
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10f);
    }

    // --- 公开方法：供房间脚本调用 ---
    public void MoveToNewRoom(Vector3 newPos)
    {
        targetPosition = newPos;
    }
}
