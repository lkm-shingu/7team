using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 20f; // ȭ�� �ӵ�
    private Vector3 target; // ȭ���� ���ư� ��ǥ
    private bool isFired = false;

    public void Initialize(Vector3 targetPosition, float attackForce)
    {
        target = targetPosition;
        isFired = true;
        Rigidbody rb = GetComponent<Rigidbody>(); // Rigidbody �ʿ�
        Vector3 shootDirection = (target - transform.position).normalized;
        rb.AddForce(shootDirection * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        if (isFired)
        {
            transform.LookAt(target); // ȭ���� ��ǥ�� ���ϵ��� ȸ��
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ �о�� ��� ����
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * 5f, ForceMode.Impulse);
            }
            Destroy(gameObject); // ȭ�� �ı�
        }
        else
        {
            Destroy(gameObject); // �ٸ� ������Ʈ�� �浹�ϸ� ȭ�� �ı�
        }
    }
}
