using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterOneTrap : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public float detectionRange = 10f; // 몬스터가 플레이어를 감지할 범위
    public float moveSpeed = 5f; // 몬스터의 이동 속도
    public float attackDistance = 1.5f; // 공격 거리
    public float disappearTime = 5f; // 사라질 시간

    private Vector3 initialPosition; // 몬스터의 초기 위치
    private bool isPlayerInSight = false;
    private float timer;

    void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (isPlayerInSight)
        {
            if (distanceToPlayer <= attackDistance)
            {
                AttackPlayer();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StopAndWait();
            }
        }
        else
        {
            if (distanceToPlayer <= detectionRange)
            {
                isPlayerInSight = true;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        // 공격 로직 (예: 플레이어에게 데미지 주기)
        // 플레이어가 밀려나는 방식 구현
        Debug.Log("플레이어를 공격합니다!");
    }

    void StopAndWait()
    {
        // 타이머를 증가시킴
        timer += Time.deltaTime;
        if (timer >= disappearTime)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // 초기 위치로 되돌리고 초기화
        transform.position = initialPosition;
        isPlayerInSight = false;
        timer = 0f;
    }

    private void OnDrawGizmos()
    {
        // 시각적으로 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
