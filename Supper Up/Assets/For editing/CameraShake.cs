using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 1f; // ��鸲 ���� �ð�
    [SerializeField] private float shakeMagnitude = 0.5f; // ��鸲 ����

    private Quaternion originalRotation; // ī�޶��� ���� ȸ�� ��

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.rotation; // �ʱ� ȸ�� ����
    }

    // public �޼���: �ܺο��� ȣ���� �� �ִ� ī�޶� ��鸲 �ڷ�ƾ
    public IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f; // ��� �ð�

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            float z = Random.Range(-shakeMagnitude, shakeMagnitude);

            // ���� ȸ�� ����
            transform.localRotation = Quaternion.Euler(x, y, z) * originalRotation;

            elapsed += Time.deltaTime; // ��� �ð� ����
            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ȸ������ ����
        StartCoroutine(Reset());
    }

    // ī�޶� ���� ȸ������ �ε巴�� �����ϴ� �ڷ�ƾ
    private IEnumerator Reset()
    {
        float elapsed = 0.0f;

        while (elapsed < 1f) // 1�� ���� ���� ȸ������ ����
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, elapsed);
            elapsed += Time.deltaTime; // ��� �ð� ����
            yield return null; // ���� �����ӱ��� ���
        }

        transform.rotation = originalRotation; // ���� ���� ���� �����Ͽ� ����
    }
}
