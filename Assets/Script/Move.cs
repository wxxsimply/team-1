using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 8f;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // 使物理更平滑
    }

    private void Update()
    {
        // 获取输入
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 翻转
        if (horizontalInput > 0 && !isFacingRight)
            Flip();
        else if (horizontalInput < 0 && isFacingRight)
            Flip();
    }

    private void FixedUpdate()
    {
        // 让移动非常稳定，不抖动
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
