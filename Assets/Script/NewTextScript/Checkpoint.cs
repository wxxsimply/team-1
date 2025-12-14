using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("设置")]
    public Color activeColor = Color.green;

    [Tooltip("【关键】请把当前房间的 CameraPoint 拖进来！")]
    public Transform roomCameraTarget; // 新增：这个存档点属于哪个房间镜头？

    private bool isActivated = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawnScript = other.GetComponent<PlayerRespawn>();

            if (respawnScript != null && roomCameraTarget != null)
            {
                // 修改：同时传入玩家位置 和 摄像机目标位置
                respawnScript.SetNewSpawnPoint(transform.position, roomCameraTarget.position);

                ActivateCheckpoint();
            }
            else
            {
                Debug.LogWarning("存档点报错：你是不是忘了拖 CameraTarget？");
            }
        }
    }

    void ActivateCheckpoint()
    {
        isActivated = true;
        if (sr != null) sr.color = activeColor;
    }
}