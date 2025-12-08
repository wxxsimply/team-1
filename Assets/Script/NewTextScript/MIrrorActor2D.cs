using UnityEngine;

public class MirrorController : MonoBehaviour
{
    public GameObject mirrorObject;     // 镜像体
    public MonoBehaviour mirrorScript;  // MirrorActor
    public MonoBehaviour afterImage;    // 残影脚本
    private bool isMirrorOn = false;

    void Update()
    {
        // 支持主键盘 5 和小键盘 5
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            ToggleMirror();
        }
    }

    void ToggleMirror()
    {
        isMirrorOn = !isMirrorOn;

        // 控制镜像物体是否显示
        mirrorObject.SetActive(isMirrorOn);

        // 同时启用/禁用镜像行为脚本
        if (mirrorScript != null)
            mirrorScript.enabled = isMirrorOn;

        // 同步控制残影开关
        if (afterImage != null)
            afterImage.enabled = isMirrorOn;
    }
}