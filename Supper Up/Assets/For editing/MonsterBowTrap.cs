using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBowTrap : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public GameObject arrowPrefab; // �߻��� ȭ�� ������
    public float detectionRange = 10f; // ���Ͱ� �÷��̾ ������ ����
    public float attackDistance = 10f; // ���� �Ÿ�
    public float attackForce = 5f; // �÷��̾ �о�� ��
    public float viewAngle = 90f; // ������ �þ߰�
    public float fireRate = 2f; // ȭ�� �߻� ����

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
            fireTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            Debug.Log("ȭ���� �߻��մϴ�!");
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

