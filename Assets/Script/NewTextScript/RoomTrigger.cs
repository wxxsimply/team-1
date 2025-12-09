using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [Tooltip("请把这个房间对应的 CameraPoint 子物体拖进来")]
    public Transform cameraTargetPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 当玩家进入这个房间的范围
        if (other.CompareTag("Player"))
        {
            // 找到主相机上的控制器
            CameraController cam = Camera.main.GetComponent<CameraController>();

            if (cam != null && cameraTargetPoint != null)
            {
                // 告诉相机：你的新家在这里
                cam.MoveToNewRoom(cameraTargetPoint.position);
            }
        }
    }
}