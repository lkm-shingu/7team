using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public CameraShake cameraShake; // ī�޶� ��鸲 ��ũ��Ʈ

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹���� ���� ī�޶� ��鸲 ����
        if (other.CompareTag("Player"))
        {
            cameraShake.StartCoroutine(cameraShake.ShakeCoroutine());
        }
    }
}

