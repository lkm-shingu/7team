using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterOneTrap : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public float detectionRange = 10f; // ���Ͱ� �÷��̾ ������ ����
    public float moveSpeed = 5f; // ������ �̵� �ӵ�
    public float attackDistance = 1.5f; // ���� �Ÿ�
    public float disappearTime = 5f; // ����� �ð�

    private Vector3 initialPosition; // ������ �ʱ� ��ġ
    private bool isPlayerInSight = false;
    private float timer;

    void Start()
    {
        initialPosition = transform.position; // �ʱ� ��ġ ����
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
        // ���� ���� (��: �÷��̾�� ������ �ֱ�)
        // �÷��̾ �з����� ��� ����
        Debug.Log("�÷��̾ �����մϴ�!");
    }

    void StopAndWait()
    {
        // Ÿ�̸Ӹ� ������Ŵ
        timer += Time.deltaTime;
        if (timer >= disappearTime)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // �ʱ� ��ġ�� �ǵ����� �ʱ�ȭ
        transform.position = initialPosition;
        isPlayerInSight = false;
        timer = 0f;
    }

    private void OnDrawGizmos()
    {
        // �ð������� ���� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
