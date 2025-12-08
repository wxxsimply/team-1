using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAppearance : MonoBehaviour
{
    [Header("需要拖入的素材")]
    [Tooltip("请把尖刺的图片素材（Sprite）拖到这里")]
    public Sprite spikeSprite;

    [Tooltip("请把传送门的图片素材（Sprite）拖到这里")]
    public Sprite portalSprite;

    // 内部变量
    private SpriteRenderer sr;
    private Sprite initialSprite; // 用来存玩家最开始的样子
    private Sprite[] possibleSprites; // 用来存所有可能的变身选项

    void Start()
    {
        // 获取自身的渲染器组件
        sr = GetComponent<SpriteRenderer>();

        // 1. 保存玩家的“初始图像”
        if (sr != null)
        {
            initialSprite = sr.sprite;
        }
        else
        {
            Debug.LogError("玩家身上没有 SpriteRenderer 组件！");
            enabled = false; // 禁用脚本防止报错
            return;
        }

        // 2. 检查是否拖入了必要的素材
        if (spikeSprite == null || portalSprite == null)
        {
            Debug.LogWarning("请在 RandomAppearance 脚本中拖入尖刺和传送门的 Sprite 素材！否则只有初始图像可选。");
        }

        // 3. 初始化素材池 (把3种可能都放进数组)
        // 数组长度为 3，分别放：初始图、尖刺图、传送门图
        possibleSprites = new Sprite[3];
        possibleSprites[0] = initialSprite;
        possibleSprites[1] = spikeSprite;
        possibleSprites[2] = portalSprite;
    }

    // 执行随机更换的核心方法
    public void ChangeSpriteRandomly()
    {
        // 安全检查
        if (sr == null || possibleSprites == null || possibleSprites.Length == 0) return;

        // --- 生成随机数 ---
        // Random.Range(min, max) 对于整数来说，包含最小值，不包含最大值。
        // 所以 Random.Range(0, 3) 会随机返回 0, 1, 或 2。
        int randomIndex = Random.Range(0, possibleSprites.Length);

        // 取出对应的图片
        Sprite selectedSprite = possibleSprites[randomIndex];

        // 如果取出的图片不是空的（防止忘记拖素材），就应用它
        if (selectedSprite != null)
        {
            sr.sprite = selectedSprite;
            Debug.Log($"变身！变成了第 {randomIndex} 号外观：{selectedSprite.name}");
        }
    }
}
