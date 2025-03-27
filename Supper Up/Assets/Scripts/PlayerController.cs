using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾��� ������ �ӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 10f;     //ȸ���ӵ�

    //ī�޶� ���� ����
    [Header("Camera Settings")]
    public Camera thirdPersonCamera;      //3��Ī

    public float cameraDistance = 5f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;

    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;
    public float mouseSenesitivity = 100.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 100.0f;

    public float radius = 5.0f;          //3��Ī ī�޶�� �÷��̾� ���� �Ÿ�
    public float minRadius = 1.0f;       //ī�޶� �ּ� �Ÿ�
    public float maxRadius = 10.0f;      //ī�޶� �ִ� �Ÿ�


    public float yMinLimit = 30;         //ī�޶� ���� ȸ�� �ּҰ�
    public float yMaxLimit = 90;         //ī�޶� ���� ȸ�� �ִ밢


    //���� ������
    private Rigidbody rb;

    public float fallingThrexhold = -0.1f;            //�������°����� ������ ���� �ӵ� �Ӱ谪

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                  //��� ���� �ִ� ���
    public const int groundCheckPoints = 5;          //���� üũ ����Ʈ ��
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;          //���콺 Ŀ���� ��װ� �����
        thirdPersonCamera.gameObject.SetActive(true);  //3��Īī�޶� Ȱ��ȭ
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

    //ī�޶� �� ĳ���� ȸ��ó���ϴ� �Լ�
    public void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity * Time.deltaTime;   //���콺 �¿� �Է�
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity * Time.deltaTime;   //���콺 ���� �Է�

        //3��Ī ī�޶� ���� ����
        CurrentX += mouseX;
        CurrentY -= mouseY;

        //���� ȸ�� ����
        CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        //ī�޶� ��ġ �� ȸ�� ���
        Vector3 dir = new Vector3(0, 0, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0.0f);
        thirdPersonCamera.transform.position = transform.position + rotation * dir;
        thirdPersonCamera.transform.LookAt(transform.position);

        //��ó��
        cameraDistance = Mathf.Clamp(cameraDistance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);
    }

    //�÷��̾� ������ ó���ϴ� �Լ�

    public void HandleJump()
    {
        //���� ��ư�� ������ ���� ���� ��
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);          //�������� ���� ���� ����
        }
    }
    //�÷��̾� �ൿó�� �Լ�
    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");         //�¿� �Է�(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //�յ� �Է�(1, -1)

        Vector3 movement;

        Vector3 cameraForward = thirdPersonCamera.transform.forward; //ī�޶� �� ����
        cameraForward.y = 0f;  //���� ���� ����
        cameraForward.Normalize();  //���� ���� ����ȭ(0~1) ������ ������ ������ش�.

        Vector3 cameraRight = thirdPersonCamera.transform.right;   //ī�޶� ������ ����
        cameraRight.y = 0f;
        cameraRight.Normalize();

        //�̵� ���� ���
        movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        //�̵��������� ĳ���� ȸ��
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

    public float GetVerticalVelocity()  //�÷��̾� y�� �ӵ�Ȯ��
    {
        return rb.velocity.y;
    }

}
