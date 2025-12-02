using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;            // 角色
    public float smoothSpeed = 5f;      // 平滑系数
    public Vector3 offset = new Vector3(0, 0, -10);  // 偏移

    private void LateUpdate()
    {
        if (target == null) return;

        // 目标位置 = 角色位置 + 偏移
        Vector3 desiredPosition = target.position + offset;

        // 平滑插值
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}