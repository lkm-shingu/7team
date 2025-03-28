using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float pushBackForce = 5f; // 플레이어를 밀어낼 힘
    public float lifespan = 1f; // 가시의 지속 시간

    public void Initialize(float damageDelay)
    {
        // 필요 시 damageDelay 관련 로직 추가
        Destroy(gameObject, lifespan); // 일정 시간이 지나면 사라짐
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = other.transform.position - transform.position;
                playerRb.AddForce(pushDirection.normalized * pushBackForce, ForceMode.Impulse);
            }
        }
    }
}

