using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾��� ������ �ӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 10f;     //ȸ���ӵ�
    //�޸��� �ӵ� ��������
    private float velocity = 1;
    private float max_velocity = 3f;
    private float speedTimer = 0;

    //���ڸ� ȸ������
    private float rotateTimer = 0;
    public float rotateTime = 3f;
    public float rotateDegree = 30;
    Quaternion toRoation = Quaternion.identity;
    private float moveDegree = 0;
    //�ִϸ��̼�
    private Animator playerAnimator;
    private float isRotateValue = 0;

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
    private const float Y_ANGLE_MAX = 85.0f;

    public float radius = 5.0f;          //3��Ī ī�޶�� �÷��̾� ���� �Ÿ�
    public float minRadius = 1.0f;       //ī�޶� �ּ� �Ÿ�
    public float maxRadius = 10.0f;      //ī�޶� �ִ� �Ÿ�

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
        playerAnimator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;          //���콺 Ŀ���� ��װ� �����
        thirdPersonCamera.gameObject.SetActive(true);  //3��Īī�޶� Ȱ��ȭ
    }

    void Update()
    {
        HandleCameraRotation();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
    }

    private void LateUpdate()
    {
        HandleCharacterRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    //ī�޶� �� ĳ���� ȸ��ó���ϴ� �Լ�
    public void HandleCameraRotation()
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
    }
    public void HandleCharacterRotation()
    {
        Vector3 cameraForward = thirdPersonCamera.transform.forward; //ī�޶� �� ����
        cameraForward.y = 0f;  //���� ���� ����
        cameraForward.Normalize();  //���� ���� ����ȭ(0~1) ������ ������ ������ش�.

        if (moveDegree > 0.1)
        {
            toRoation = Quaternion.LookRotation(cameraForward, Vector2.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRoation, rotationSpeed * Time.deltaTime);
        }

        //ȸ��ó���� �� �������ϱ�
        float dot = Vector3.Dot(transform.forward, cameraForward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //n���Ŀ� ī�޶�������� ȸ��
        if (angle >= rotateDegree&& moveDegree < 0.1)
        {
            rotateTimer += Time.deltaTime;
            if (rotateTimer > rotateTime)
            {
                toRoation = Quaternion.LookRotation(cameraForward, Vector2.up);
                rotateTimer = 0;
                isRotateValue = 1;
                Vector3 left = - transform.right;
                float temp = Vector3.Angle(cameraForward, left);
                if (temp <= 90)
                {
                    playerAnimator.SetTrigger("Lturn");
                }
                else if(temp <= 180)
                {
                    playerAnimator.SetTrigger("Rturn");
                }
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, toRoation, rotationSpeed * Time.deltaTime);

        if (isRotateValue > 0) isRotateValue += rotationSpeed * Time.deltaTime;
    }

    //�÷��̾� ������ ó���ϴ� �Լ�
    public void HandleJump()
    {
        //���� ��ư�� ������ ���� ���� ��
        if (IsGrounded())
        {
            playerAnimator.SetBool("IsJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);          //�������� ���� ���� ����
        }
    }
    //�÷��̾� �ൿó�� �Լ�
    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");         //�¿� �Է�(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //�յ� �Է�(1, -1)

        if(moveVertical > 0.95 || moveVertical < -0.95)
        {
            if (speedTimer <= max_velocity)
            {
                speedTimer += Time.deltaTime;
            }
            velocity = speedTimer;
        }
        else
        {
            speedTimer = 0;
            velocity = speedTimer;
        }
        velocity = Mathf.Clamp(velocity, 1, max_velocity);
        Debug.Log(velocity);
        //�ִϸ��̼�
        playerAnimator.SetFloat("FMove", moveVertical * velocity);
        playerAnimator.SetFloat("RMove", moveHorizontal);

        //�̵� ���� ���
        Vector3 movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
        moveDegree = movement.magnitude;
        rb.MovePosition(rb.position + movement * (moveSpeed + velocity) * Time.deltaTime);
    }

    public bool isFalling()
    {
        return rb.velocity.y < fallingThrexhold && IsGrounded();
    }

    public bool IsGrounded()
    {
        Vector3 temp = transform.position;
        temp.y += 1;
        return Physics.Raycast(temp, Vector3.down, 1f);
    }

    public float GetVerticalVelocity()  //�÷��̾� y�� �ӵ�Ȯ��
    {
        return rb.velocity.y;
    }

    private void OnDrawGizmos()
    {
        Vector3 cameraForward = thirdPersonCamera.transform.forward; //ī�޶� �� ����
        cameraForward.y = 0f;  //���� ���� ����
        cameraForward.Normalize();  //���� ���� ����ȭ(0~1) ������ ������ ������ش�.

        Gizmos.color = Color.yellow;
        Debug.DrawRay(transform.position, cameraForward);

        Gizmos.color = Color.red;
        Debug.DrawRay(transform.position, transform.forward);
    }

}
