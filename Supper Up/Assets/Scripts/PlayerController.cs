using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 10f;     //회전속도

    //카메라 설정 변수
    [Header("Camera Settings")]
    public Camera thirdPersonCamera;      //3인칭

    public float cameraDistance = 5f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;

    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;
    public float mouseSenesitivity = 100.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 100.0f;

    public float radius = 5.0f;          //3인칭 카메라와 플레이어 간의 거리
    public float minRadius = 1.0f;       //카메라 최소 거리
    public float maxRadius = 10.0f;      //카메라 최대 거리


    public float yMinLimit = 30;         //카메라 수직 회전 최소각
    public float yMaxLimit = 90;         //카메라 수직 회전 최대각


    //내부 변수들
    private Rigidbody rb;

    public float fallingThrexhold = -0.1f;            //떨어지는것으로 간주할 수직 속도 임계값

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                  //등반 가능 최대 경사
    public const int groundCheckPoints = 5;          //지면 체크 포인트 수
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;          //마우스 커서를 잠그고 숨긴다
        thirdPersonCamera.gameObject.SetActive(true);  //3인칭카메라 활성화
    }

    void Update()
    {
        HandleRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    //카메라 및 캐릭터 회전처리하는 함수
    public void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity * Time.deltaTime;   //마우스 좌우 입력
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity * Time.deltaTime;   //마우스 상하 입력

        //3인칭 카메라 로직 수정
        CurrentX += mouseX;
        CurrentY -= mouseY;

        //수직 회전 제한
        CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        //카메라 위치 및 회전 계산
        Vector3 dir = new Vector3(0, 0, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0.0f);
        thirdPersonCamera.transform.position = transform.position + rotation * dir;
        thirdPersonCamera.transform.LookAt(transform.position);

        //줌처리
        cameraDistance = Mathf.Clamp(cameraDistance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);
    }

    //플레이어 점프를 처리하는 함수

    public void HandleJump()
    {
        //접프 버튼을 누르고 땅에 있을 때
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);          //위쪽으로 힘을 가해 점프
        }
    }
    //플레이어 행동처리 함수
    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");         //좌우 입력(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //앞뒤 입력(1, -1)

        Vector3 movement;

        Vector3 cameraForward = thirdPersonCamera.transform.forward; //카메라 앞 방향
        cameraForward.y = 0f;  //수직 방향 제거
        cameraForward.Normalize();  //방향 백터 정규화(0~1) 사이의 값으로 만들어준다.

        Vector3 cameraRight = thirdPersonCamera.transform.right;   //카메라 오른쪽 방향
        cameraRight.y = 0f;
        cameraRight.Normalize();

        //이동 백터 계산
        movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        //이동방향으로 캐릭터 회전
        if (movement.magnitude > 0.1f)
        {
            Quaternion toRoation = Quaternion.LookRotation(movement, Vector2.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRoation, rotationSpeed * Time.deltaTime);
        }
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    public bool isFalling()
    {
        return rb.velocity.y < fallingThrexhold && IsGrounded();
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }

    public float GetVerticalVelocity()  //플레이어 y축 속도확인
    {
        return rb.velocity.y;
    }

}
