using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraAction : MonoBehaviour
{
    public GameObject Target;               // ī�޶� ����ٴ� Ÿ��

    public float angleX = 0.0f;
    public float angleY = 0.0f;
    public float angleZ = 0.0f;

    public float offsetX = 0f;            // ī�޶��� x��ǥ
    public float offsetY = 2f;           // ī�޶��� y��ǥ
    public float offsetZ = -10.0f;          // ī�޶��� z��ǥ
    public float CameraSpeed = 10.0f;       // ī�޶��� �ӵ�
    Vector3 TargetPos;                      // Ÿ���� ��ġ

    private void FixedUpdate()
    {
        TargetPos = new Vector3(Target.transform.position.x + offsetX,
                                Target.transform.position.y + offsetY,
                                Target.transform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);

        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
    }
}