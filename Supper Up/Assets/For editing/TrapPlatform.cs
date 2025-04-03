using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public CameraShake cameraShake; // 카메라 흔들림 스크립트

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했을 때만 카메라 흔들림 시작
        if (other.CompareTag("Player"))
        {
            cameraShake.StartCoroutine(cameraShake.ShakeCoroutine());
        }
    }
}

