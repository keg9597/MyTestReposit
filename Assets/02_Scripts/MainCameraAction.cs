using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraAction : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float angleX = 0.0f;
    public float angleY = 0.0f;
    public float angleZ = 0.0f;

    public float offsetX = 0f;            // 카메라의 x좌표
    public float offsetY = 2f;           // 카메라의 y좌표
    public float offsetZ = -10.0f;          // 카메라의 z좌표
    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치

    private void FixedUpdate()
    {
        TargetPos = new Vector3(Target.transform.position.x + offsetX,
                                Target.transform.position.y + offsetY,
                                Target.transform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);

        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
    }
}
