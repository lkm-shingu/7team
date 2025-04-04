using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBowTrap : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public GameObject arrowPrefab; // 발사할 화살 프리팹
    public float detectionRange = 10f; // 몬스터가 플레이어를 감지할 범위
    public float attackDistance = 10f; // 공격 거리
    public float attackForce = 5f; // 플레이어를 밀어내는 힘
    public float viewAngle = 90f; // 몬스터의 시야각
    public float fireRate = 2f; // 화살 발사 간격

    private float fireTimer;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRange && IsPlayerInView())
        {
            if (distanceToPlayer <= attackDistance)
            {
                FireArrow();
            }
        }
    }

    void FireArrow()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            arrowController.Initialize(player.position, attackForce);
            fireTimer = 0f; // 타이머 초기화
            Debug.Log("화살을 발사합니다!");
        }
    }

    private bool IsPlayerInView()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);
        return angleBetween < viewAngle / 2f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * detectionRange);
    }
}

