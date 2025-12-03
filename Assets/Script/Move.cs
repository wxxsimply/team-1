using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
// 建议也加上这个，防止忘记加 Animator 组件
[RequireComponent(typeof(Animator))]
public class Move : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 8f;

    [Header("跳跃参数")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim; // [新增 1] 定义 Animator 变量
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // [新增 2] 获取 Animator 组件
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        // 1. 地面检测
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. 获取移动输入
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // [新增 3] 动画控制逻辑
        // 只要输入不为 0 (代表按下了左或右)，就是跑步状态
        if (horizontalInput != 0)
        {
            anim.SetBool("IsRun", true);
        }
        else
        {
            anim.SetBool("IsRun", false);
        }

        // 3. 跳跃输入
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // 如果你有跳跃动画，可以在这里加: anim.SetTrigger("Jump");
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
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}