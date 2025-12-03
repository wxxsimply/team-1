using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LighterFlicker : MonoBehaviour
{
    private Light2D fireLight;

    [Header("闪烁参数")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 10f; // 闪烁速度

    void Awake()
    {
        fireLight = GetComponent<Light2D>();
    }

    void Update()
    {
        if (fireLight == null) return;

        // 使用柏林噪声 (Perlin Noise) 生成自然的随机波动
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        // 将噪声映射到最小和最大强度之间
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
