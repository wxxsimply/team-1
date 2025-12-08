using UnityEngine;

public class GhostTrail2D : MonoBehaviour
{
    [Header("残影参数")]
    public float spawnInterval = 0.01f;   // 生成间隔
    public float ghostLifeTime = 0.3f;    // 残影存在时间
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);

    [Header("渲染器")]
    public SpriteRenderer targetRenderer; // 角色 SpriteRenderer

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            CreateGhost();
        }
    }

    void CreateGhost()
    {
        if (targetRenderer == null) return;

        // 创建残影物体
        GameObject ghost = new GameObject("Ghost2D");
        SpriteRenderer sr = ghost.AddComponent<SpriteRenderer>();

        // 复制当前精灵
        sr.sprite = targetRenderer.sprite;
        sr.flipX = targetRenderer.flipX;
        sr.flipY = targetRenderer.flipY;

        sr.color = ghostColor;
        sr.sortingLayerID = targetRenderer.sortingLayerID;
        sr.sortingOrder = targetRenderer.sortingOrder - 1;

        // 位置/旋转/缩放同步
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
        ghost.transform.localScale = transform.localScale;

        // 添加淡出脚本
        GhostFade2D fade = ghost.AddComponent<GhostFade2D>();
        fade.lifeTime = ghostLifeTime;
        fade.startColor = ghostColor;
    }
}
