using UnityEngine;

public class MirrorActor : MonoBehaviour
{
    public Transform target;  // 主角
    public float k = 0f;      // 对称轴 x = k

    private Animator mirrorAnimator;
    private Animator targetAnimator;

    // 【新增：2D 用 SpriteRenderer】
    private SpriteRenderer[] spriteRenderers;

    // 【新增：残影参数】
    public float ghostInterval = 0.05f;
    public float ghostLife = 0.3f;
    public Color ghostColor = new Color(1, 1, 1, 0.5f);

    private float ghostTimer = 0f;

    // 1. 修改变量初始值：默认为 true (开启)
    private bool isActive = true;

    void Start()
    {
        targetAnimator = target.GetComponent<Animator>();
        mirrorAnimator = GetComponent<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (targetAnimator != null && mirrorAnimator != null)
            mirrorAnimator.runtimeAnimatorController = targetAnimator.runtimeAnimatorController;

        // 2. 修改 Start 方法：游戏开始时直接设为可见 (true)
        SetVisible(true);
    }

    void Update()
    {
        // 3. 删除或注释掉 Update 里的按键检测代码
        // 这样玩家按下 5 键就不会有任何反应，也就无法关闭镜像了

        /* if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            isActive = !isActive;
            SetVisible(isActive);
        }
        */
    }

    void LateUpdate()
    {
        if (!isActive || target == null) 
            return;

        // ========= 1. 镜像位置 =========
        Vector3 pos = target.position;
        pos.x = 2f * k - pos.x;
        transform.position = pos;

        // ========= 2. 镜像旋转（2D 用不到 Y/Z 翻转，但保留）=========
        Vector3 rot = target.rotation.eulerAngles;
        rot.y = -rot.y;
        rot.z = -rot.z;
        transform.rotation = Quaternion.Euler(rot);

        // ========= 3. 镜像缩放 =========
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

        // ========= 5. 残影生成（新增！）=========
        ghostTimer += Time.deltaTime;
        if (ghostTimer >= ghostInterval)
        {
            ghostTimer = 0f;
            CreateGhost();
        }
    }

    // ------------------ 生成残影（新增功能） ------------------
    void CreateGhost()
    {
        foreach (var sr in spriteRenderers)
        {
            GameObject ghost = new GameObject("MirrorGhost");
            SpriteRenderer gsr = ghost.AddComponent<SpriteRenderer>();

            gsr.sprite = sr.sprite;
            gsr.flipX = sr.flipX;
            gsr.flipY = sr.flipY;
            gsr.color = ghostColor;

            ghost.transform.position = sr.transform.position;
            ghost.transform.rotation = sr.transform.rotation;
            ghost.transform.localScale = sr.transform.lossyScale;

            // 添加淡出脚本
            var fade = ghost.AddComponent<GhostFade2D>();
            fade.lifeTime = ghostLife;
            fade.startColor = ghostColor;
        }
    }

    // ------------------ 残影淡出脚本（内置） ------------------
    class GhostFade2D : MonoBehaviour
    {
        public float lifeTime = 0.3f;
        public Color startColor;

        private SpriteRenderer sr;
        private float t = 0;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            t += Time.deltaTime;
            float rate = t / lifeTime;

            Color c = startColor;
            c.a = Mathf.Lerp(startColor.a, 0f, rate);
            sr.color = c;

            if (t >= lifeTime)
                Destroy(gameObject);
        }
    }

    // ------------------ 2D 显示/隐藏 ------------------
    void SetVisible(bool v)
    {
        foreach (var sr in spriteRenderers)
            sr.enabled = v;
    }
}
