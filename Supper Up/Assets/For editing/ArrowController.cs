using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 20f; // 화살 속도
    private Vector3 target; // 화살이 날아갈 목표
    private bool isFired = false;

    public void Initialize(Vector3 targetPosition, float attackForce)
    {
        target = targetPosition;
        isFired = true;
        Rigidbody rb = GetComponent<Rigidbody>(); // Rigidbody 필요
        Vector3 shootDirection = (target - transform.position).normalized;
        rb.AddForce(shootDirection * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        if (isFired)
        {
            transform.LookAt(target); // 화살이 목표를 향하도록 회전
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어를 밀어내는 방식 구현
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * 5f, ForceMode.Impulse);
            }
            Destroy(gameObject); // 화살 파괴
        }
        else
        {
            Destroy(gameObject); // 다른 오브젝트와 충돌하면 화살 파괴
        }
    }
}
