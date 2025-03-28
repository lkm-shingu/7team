using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float pushBackForce = 5f; // �÷��̾ �о ��
    public float lifespan = 1f; // ������ ���� �ð�

    public void Initialize(float damageDelay)
    {
        // �ʿ� �� damageDelay ���� ���� �߰�
        Destroy(gameObject, lifespan); // ���� �ð��� ������ �����
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

