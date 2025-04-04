using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //ī�޶� ���� ����
    [Header("Camera Settings")]
    public GameObject player;
    public float cameraDistance = 5f;

    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;
    public float mouseSenesitivity = 100.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 85.0f;

    public float radius = 5.0f;          //3��Ī ī�޶�� �÷��̾� ���� �Ÿ�

    void Update()
    {
        CameraRotation();
    }

    //ī�޶� �� ĳ���� ȸ��ó���ϴ� �Լ�
    public void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity * Time.deltaTime;

        CurrentX += mouseX;
        CurrentY -= mouseY;

        CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        //ī�޶� ��ġ �� ȸ�� ���
        Vector3 dir = new Vector3(0, 0, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0.0f);
        transform.position = player.transform.position + rotation * dir;
        transform.LookAt(player.transform.position);
    }
}
