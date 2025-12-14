using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 如果撞到的是【真身】
        if (other.CompareTag("Player"))
        {
            PlayerRespawn playerScript = other.GetComponent<PlayerRespawn>();
            if (playerScript != null)
            {
                Debug.Log("被固定尖刺扎死！");
                playerScript.Die();
            }

            // 2. 如果撞到的是【镜像】(Mirror)
            MirrorLink mirrorScript = other.GetComponent<MirrorLink>();
            if (mirrorScript != null)
            {
                Debug.Log("镜像撞刺，本体死亡！");
                mirrorScript.Die();
            }
        }
    }
}
