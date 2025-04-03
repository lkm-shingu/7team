using UnityEngine;

public class MonsterOneTrap : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public float detectionRange = 10f; // ���Ͱ� �÷��̾ ������ ����
    public float moveSpeed = 5f; // ������ �̵� �ӵ�
    public float attackDistance = 1.5f; // ���� �Ÿ�
    public float disappearTime = 5f; // ����� �ð�
    public float attackForce = 5f; // �÷��̾ �о�� ��
    public float viewAngle = 70f; // ������ �þ߰� (140���� ������ ������ ���������� �� ���⿡ ���� 70���� ����)

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
            if (distanceToPlayer <= detectionRange && IsPlayerInView())
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
        // �÷��̾ �о�� ��� ����
        Vector3 pushDirection = (player.position - transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(pushDirection * attackForce, ForceMode.Impulse);
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

    private bool IsPlayerInView()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);
        return angleBetween < viewAngle / 2f;
    }

    private void OnDrawGizmos()
    {
        // �ð������� ���� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        // �þ߰� ǥ�� (����ȭ�� ����)
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * detectionRange);
    }
}
