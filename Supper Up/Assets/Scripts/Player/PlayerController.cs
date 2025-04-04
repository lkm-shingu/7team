using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera thirdPersonCamera;      //3인칭

    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public MovementController movementController;

    //내부 변수들
    private Vector3 movement = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;          //마우스 커서를 잠그고 숨긴다
        StartCoroutine(movementController.Falling());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) movementController.Jumping();

        movementController.Landing();

        movementController.Rotate();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }

    //플레이어 행동처리 함수
    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");         //좌우 입력(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //앞뒤 입력(1, -1)

        movementController.Move(moveVertical, moveHorizontal, movement);

        //이동 백터 계산
        if (!movementController.onRotate) movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
        else movement = transform.forward * moveVertical;

        movementController.RotateDiagonal(moveHorizontal, moveVertical, movement);
    }

    private void OnDrawGizmos()
    {
        Vector3 cameraForward = thirdPersonCamera.transform.forward; //카메라 앞 방향
        cameraForward.y = 0f;  //수직 방향 제거
        cameraForward.Normalize();  //방향 백터 정규화(0~1) 사이의 값으로 만들어준다.

        Gizmos.color = Color.yellow;
        Debug.DrawRay(transform.position, cameraForward);

        Gizmos.color = Color.red;
        Debug.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.red;
        Debug.DrawRay(transform.position, Vector3.down * 2f);
    }

}
