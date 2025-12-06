using UnityEngine;

public class MirrorStaticObject : MonoBehaviour
{
    public Transform target;  // 原角色
    public float k = 0f;      // 镜像平面 x = k

    [ContextMenu("Create Static Mirror")]
    public void CreateMirror()
    {
        if (target == null)
        {
            Debug.LogError("请把 target 拖到 target 变量里！");
            return;
        }

        // -------- 1. 创建空物体 --------
        GameObject mirror = new GameObject(target.name + "_Mirror");
        mirror.transform.parent = target.parent;

        // -------- 2. 复制 Mesh 和材质（仅外观）--------
        MeshFilter mfTarget = target.GetComponent<MeshFilter>();
        MeshRenderer mrTarget = target.GetComponent<MeshRenderer>();

        if (mfTarget != null)
        {
            MeshFilter mf = mirror.AddComponent<MeshFilter>();
            mf.sharedMesh = mfTarget.sharedMesh;
        }

        if (mrTarget != null)
        {
            MeshRenderer mr = mirror.AddComponent<MeshRenderer>();
            mr.sharedMaterials = mrTarget.sharedMaterials;
        }

        // -------- 3. 位置镜像（关于 x = k）--------
        Vector3 pos = target.position;
        pos.x = 2f * k - pos.x;
        mirror.transform.position = pos;

        // -------- 4. 旋转镜像 --------
        Vector3 rot = target.eulerAngles;
        rot.y = -rot.y;
        rot.z = -rot.z;
        mirror.transform.eulerAngles = rot;

        // -------- 5. 缩放镜像（翻转 X）--------
        Vector3 scale = target.localScale;
        scale.x = -scale.x;
        mirror.transform.localScale = scale;

        Debug.Log("已生成静态镜像模型（无任何移动行为）!");
    }
}
