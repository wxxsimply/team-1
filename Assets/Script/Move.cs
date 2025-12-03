using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 8f;

    [Header("跳跃参数")]
    public float jumpForce = 12f;          // 跳跃力度
    public Transform groundCheck;          // 脚底检测点的位置
    public float groundCheckRadius = 0.2f; // 检测半径
    public LayerMask groundLayer;          // 什么是“地面”？(图层设置)

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded;               // 当前是否在地面上

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        // 1. 地面检测
        // 在脚底画一个小圆圈，看看有没有碰到属于 "groundLayer" 的物体
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. 获取移动输入
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 3. 跳跃输入 (按下空格键 且 在地面上)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // 直接修改 Y 轴速度实现跳跃，保留当前的 X 轴速度
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 4. 翻转角色
        if (horizontalInput > 0 && !isFacingRight)
            Flip();
        else if (horizontalInput < 0 && isFacingRight)
            Flip();
    }

    private void FixedUpdate()
    {
        // 5. 物理移动
        // 注意这里使用 rb.velocity.y，这样不会覆盖掉跳跃产生的垂直速度
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 辅助功能：在编辑器里画出检测范围，方便调试
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
