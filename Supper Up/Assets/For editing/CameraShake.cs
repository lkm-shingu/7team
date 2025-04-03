using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 1f; // 흔들림 지속 시간
    [SerializeField] private float shakeMagnitude = 0.5f; // 흔들림 강도

    private Quaternion originalRotation; // 카메라의 원래 회전 값

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.rotation; // 초기 회전 저장
    }

    // public 메서드: 외부에서 호출할 수 있는 카메라 흔들림 코루틴
    public IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f; // 경과 시간

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);
            float z = Random.Range(-shakeMagnitude, shakeMagnitude);

            // 랜덤 회전 적용
            transform.localRotation = Quaternion.Euler(x, y, z) * originalRotation;

            elapsed += Time.deltaTime; // 경과 시간 갱신
            yield return null; // 다음 프레임까지 대기
        }

        // 원래 회전으로 복원
        StartCoroutine(Reset());
    }

    // 카메라를 원래 회전으로 부드럽게 복원하는 코루틴
    private IEnumerator Reset()
    {
        float elapsed = 0.0f;

        while (elapsed < 1f) // 1초 동안 원래 회전으로 복원
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, elapsed);
            elapsed += Time.deltaTime; // 경과 시간 갱신
            yield return null; // 다음 프레임까지 대기
        }

        transform.rotation = originalRotation; // 직접 원래 값을 설정하여 보장
    }
}
