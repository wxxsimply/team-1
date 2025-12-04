using UnityEngine;

public class FloatFollower : MonoBehaviour
{
    [Header("初始随机漂浮")]
    public float floatSpeed = 2f;        // 随机移动速度
    public float floatRange = 0.5f;      // 漂浮幅度
    public float directionChangeTime = 1.2f;

    [Header("追踪参数（碰到角色后开启）")]
    public Transform target;             // 角色 Transform
    public float followSpeedX = 4f;
    public float followSpeedY = 2f;
    public float stopDistanceX = 1f;     // X 接近角色就停止移动
    public float hoverHeight = 0.3f;     // Y 的目标高度偏移

    private Vector2 randomDir;           // 初始移动方向
    private float changeTimer;
    private bool activated = false;      // 是否已被角色激活（首次碰撞）

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;             // 永远不受重力影响
        rb.freezeRotation = true;

        PickNewRandomDirection();
    }

    private void Update()
    {
        if (!activated)
        {
            RandomFloating();
        }
        else
        {
            FollowPlayer();
        }
    }

    // ============================
    // ⭐ 1. 初始随机漂浮逻辑
    // ============================
    private void RandomFloating()
    {
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0f)
        {
            PickNewRandomDirection();
        }

        rb.velocity = randomDir * floatSpeed;
    }

    private void PickNewRandomDirection()
    {
        randomDir = Random.insideUnitCircle.normalized;
        changeTimer = directionChangeTime;
    }

    // ============================
    // ⭐ 2. 追踪玩家（触发后）
    // ============================
    private void FollowPlayer()
    {
        if (target == null) return;

        Vector3 pos = transform.position;
        Vector3 targetPos = target.position;

        // ----- X 追踪 -----
        float dx = targetPos.x - pos.x;

        if (Mathf.Abs(dx) > stopDistanceX)
        {
            float moveX = Mathf.Sign(dx) * followSpeedX;
            rb.velocity = new Vector2(moveX, rb.velocity.y);
        }
        else
        {
            // 进入停止区域 → 停止 X
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // ----- Y 追踪 -----
        float desiredY = targetPos.y + hoverHeight;
        float moveY = (desiredY - pos.y) * followSpeedY;
        rb.velocity = new Vector2(rb.velocity.x, moveY);
    }

    // ============================
    // ⭐ 3. 第一次碰到角色 → 激活跟随
    // ============================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            activated = true;
            target = collision.transform;  // 设置要跟随的角色
        }
    }
}
