using UnityEngine;

public class MirrorActor : MonoBehaviour
{
    public Transform target;  // 主角
    public float k = 0f;      // 对称轴 x = k

    private Animator mirrorAnimator;
    private Animator targetAnimator;
    private Renderer[] renderers;

    private bool isActive = false;  // 是否启用镜像

    void Start()
    {
        targetAnimator = target.GetComponent<Animator>();
        mirrorAnimator = GetComponent<Animator>();
        renderers = GetComponentsInChildren<Renderer>();

        // 拷贝动画控制器
        if (targetAnimator != null && mirrorAnimator != null)
        {
            mirrorAnimator.runtimeAnimatorController = targetAnimator.runtimeAnimatorController;
        }

        // 初始化为关闭状态（隐藏）
        SetVisible(false);
    }

    void Update()
    {
        // 按键 5 切换镜像开关
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            isActive = !isActive;
            SetVisible(isActive);
        }
    }

    void LateUpdate()
    {
        if (!isActive || target == null)
            return;

        // ========= 1. 位置镜像 =========
        Vector3 pos = target.position;
        pos.x = 2f * k - pos.x;
        transform.position = pos;

        // ========= 2. 旋转镜像 =========
        Vector3 rot = target.rotation.eulerAngles;
        rot.y = -rot.y;
        rot.z = -rot.z;
        transform.rotation = Quaternion.Euler(rot);

        // ========= 3. 缩放镜像 =========
        Vector3 scale = target.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;

        // ========= 4. 动画同步 =========
        if (targetAnimator != null && mirrorAnimator != null)
        {
            mirrorAnimator.speed = targetAnimator.speed;

            for (int i = 0; i < targetAnimator.parameterCount; i++)
            {
                var p = targetAnimator.parameters[i];

                switch (p.type)
                {
                    case AnimatorControllerParameterType.Float:
                        mirrorAnimator.SetFloat(p.name, targetAnimator.GetFloat(p.name)); break;

                    case AnimatorControllerParameterType.Bool:
                        mirrorAnimator.SetBool(p.name, targetAnimator.GetBool(p.name)); break;

                    case AnimatorControllerParameterType.Int:
                        mirrorAnimator.SetInteger(p.name, targetAnimator.GetInteger(p.name)); break;

                    case AnimatorControllerParameterType.Trigger:
                        if (targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.1f)
                            mirrorAnimator.SetTrigger(p.name);
                        break;
                }
            }
        }
    }

    // ---- 隐藏或显示所有 MeshRenderer ----
    void SetVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}
