using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform targetPosition;  // 传送目标位置

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && targetPosition != null)
        {
            other.transform.position = targetPosition.position;
        }
    }
}
