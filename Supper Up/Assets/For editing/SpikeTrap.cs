using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public GameObject spikePrefab; // 가시 오브젝트 프리팹
    public Transform spawnPoint;    // 가시가 나올 위치
    public float damageDelay = 0.5f; // 플레이어에게 데미지를 줄 때의 지연시간
    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            ActivateTrap();
            // 플레이어에게 데미지를 줄 수 있는 부분을 추가할 수 있습니다.
        }
    }

    void ActivateTrap()
    {
        GameObject spike = Instantiate(spikePrefab, spawnPoint.position, Quaternion.identity);
        Spike spikeScript = spike.GetComponent<Spike>();
        spikeScript.Initialize(damageDelay);
    }
}
