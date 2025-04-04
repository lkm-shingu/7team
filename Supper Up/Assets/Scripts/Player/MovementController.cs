using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera thirdPersonCamera;      //3인칭

    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 10f;     //회전속도
    private Vector3 moveVector;

    [Header("Player Rotation")]
    public float velocity = 1;
    private float max_velocity = 3f;
    private float min_velocity = 1f;
    private float speedTimer = 0;
    //제자리 회전변수
    private float rotateTimer = 0;
    public float rotateTime = 3f;
    public float rotateDegree = 30;
    Quaternion toRoation = Quaternion.identity;
    private float moveDegree = 0;
    public bool onRotate = false;

    [Header("Ground Check Setting")]
    public float fallingThrexhold = -0.1f;            //떨어지는것으로 간주할 수직 속도 임계값
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                  //등반 가능 최대 경사
    private bool isFalling = false;
    private bool isJumping = false;

    //내부 변수들
    private Rigidbody rb;
    private Animator playerAnimator;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }


    public void Move(float moveVertical, float moveHorizontal, Vector3 movement)
    {
        moveVector = movement;
        AdjustSpeed(moveVertical);
        //애니메이션
        playerAnimator.SetFloat("FMove", moveVertical * velocity);
        playerAnimator.SetFloat("RMove", moveHorizontal);

        moveDegree = movement.magnitude;

        rb.MovePosition(rb.position + movement * (moveSpeed + velocity) * Time.deltaTime);
    }

    //시간이 흐름에 따라 속도를 올려주는 코드
    private void AdjustSpeed(float moveVertical)
    {
        if (Mathf.Abs(moveVertical) > 0.95f)
        {
            speedTimer = Mathf.Min(speedTimer += Time.deltaTime * 1.5f, max_velocity);
        }
        else
        {
            speedTimer = Mathf.Max(speedTimer -= Time.deltaTime * 2f, 0);
        }
        velocity = Mathf.Clamp(speedTimer, min_velocity, max_velocity);
    }

    public void Rotate()
    {
        Vector3 cameraForward = thirdPersonCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (moveDegree > 0.1 && !onRotate) toRoation = Quaternion.LookRotation(cameraForward, Vector2.up); //카메라에 따라서 캐릭터회전

        float dot = Vector3.Dot(transform.forward, cameraForward);               //회전처리를 할 각구하기
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle >= rotateDegree && moveDegree < 0.1)                           //n초후에 카메라방향으로 회전
        {
            rotateTimer += Time.deltaTime;
            if (rotateTimer > rotateTime)
            {
                toRoation = Quaternion.LookRotation(cameraForward, Vector2.up);
                rotateTimer = 0;
                Vector3 left = -transform.right;
                float temp = Vector3.Angle(cameraForward, left);

                if (temp <= 90) playerAnimator.SetTrigger("Lturn");
                else if (temp <= 180) playerAnimator.SetTrigger("Rturn");
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, toRoation, rotationSpeed * Time.deltaTime);
    }

    public void RotateDiagonal(float moveHorizontal, float moveVertical, Vector3 movement)
    {
        Debug.Log(onRotate);
        if (!onRotate)
        {
            if (velocity < 1.5 || Mathf.Abs(moveHorizontal) < 0.3f) return;
            Vector3 temp = movement;
            if (moveVertical < -0.9f) temp = -temp;
            toRoation = Quaternion.LookRotation(temp, Vector2.up);
            onRotate = true;
        }
        else if(Mathf.Abs(moveHorizontal) < 0.3f) onRotate = false;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            playerAnimator.SetBool("IsJumping", true);
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce + moveVector * velocity * 3.5f, ForceMode.Impulse);          //위쪽으로 힘을 가해 점프
        }
    }

    public void NotGroundAction()
    {
        if (!isJumping) return;
        if (CheckDistance() > 3f)
        {
            playerAnimator.SetBool("IsFalling", true);
            isFalling = true;
        }
        else
        {
            if (isFalling)
            {
                playerAnimator.SetBool("IsFalling", false);
                isFalling = false;
            }
            else
            {
                playerAnimator.SetBool("IsJumping", false);
            }
        }
    }
    private IEnumerator ResetValues()
    {
        isJumping = false;
        yield return new WaitForSeconds(1f);
        playerAnimator.SetBool("IsJumping", false);
        playerAnimator.SetBool("IsFalling", false);
    }

    public bool IsFalling()
    {
        return rb.velocity.y < fallingThrexhold && IsGrounded();
    }

    private float CheckDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) return hit.distance;
        return 10f;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2f);
    }
}
