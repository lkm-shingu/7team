using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public GameObject spikePrefab; // ���� ������Ʈ ������
    public Transform spawnPoint;    // ���ð� ���� ��ġ
    public float damageDelay = 0.5f; // �÷��̾�� �������� �� ���� �����ð�
    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            ActivateTrap();
            // �÷��̾�� �������� �� �� �ִ� �κ��� �߰��� �� �ֽ��ϴ�.
        }
    }

    void ActivateTrap()
    {
        GameObject spike = Instantiate(spikePrefab, spawnPoint.position, Quaternion.identity);
        Spike spikeScript = spike.GetComponent<Spike>();
        spikeScript.Initialize(damageDelay);
    }
}
