using UnityEngine;

public class GhostFade2D : MonoBehaviour
{
    public float lifeTime = 0.3f;
    public Color startColor;

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / lifeTime;

        Color c = sr.color;
        c.a = Mathf.Lerp(startColor.a, 0f, t);
        sr.color = c;

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}