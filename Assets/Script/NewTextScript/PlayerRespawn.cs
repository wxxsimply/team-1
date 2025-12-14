using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 如果你想重置整个关卡，需要这个

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;       // 玩家复活坐标
    private Vector3 cameraSpawnPoint; // 新增：摄像机复活坐标
    private Rigidbody2D rb;

    void Start()
    {
        spawnPoint = transform.position;
        // 默认摄像机位置就是当前摄像机的位置
        cameraSpawnPoint = Camera.main.transform.position;

        rb = GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        Debug.Log("玩家死亡，回溯！");

        // 1. 玩家归位
        transform.position = spawnPoint;
        if (rb != null) rb.velocity = Vector2.zero;

        // 2. 新增：摄像机归位
        // 找到摄像机脚本，强行把镜头切过去
        CameraController cam = Camera.main.GetComponent<CameraController>();
        if (cam != null)
        {
            cam.MoveToNewRoom(cameraSpawnPoint);
        }
    }

    // 修改这个方法，接收两个参数
    public void SetNewSpawnPoint(Vector3 newPlayerPos, Vector3 newCameraPos)
    {
        spawnPoint = newPlayerPos;
        cameraSpawnPoint = newCameraPos; // 记住这个存档点对应的镜头位置
    }
}