using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    // 二段跳相关
    public int maxJumpCount = 2; // 可以跳几段
    private int jumpCount = 0;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        // 1. 地面检测
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ★ 贴地后重置跳跃次数
        if (isGrounded)
            jumpCount = 0;

        // 2. 获取移动
        horizontalInput = Input.GetAxisRaw("Horizontal");

        anim.SetBool("IsRun", horizontalInput != 0);

        // 3. 跳跃
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;

            // 如果你有跳跃动画，可以加
            // anim.SetTrigger("Jump");
        }

        // 4. 翻转角色
        if (horizontalInput > 0 && !isFacingRight)
            Flip();
        else if (horizontalInput < 0 && isFacingRight)
            Flip();
    }

    private void FixedUpdate()
    {
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
