using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 카메라가 따라갈 대상
    public float rotationSpeed = 5f; // 회전 속도
    public float distance = 10f; // 타겟으로부터의 거리
    public float minVerticalAngle = -80f; // 수직 회전 최소 각도
    public float maxVerticalAngle = 80f; // 수직 회전 최대 각도

    private float currentX = 0f;
    private float currentY = 0f;
    private Quaternion originalRotation;
    private float tempX = 0f;
    private float tempY = 0f;
    private bool isTemporaryRotation = false;

    //카메라가 통과 못할 레이어
    public LayerMask Ground;

    void Start()
    {
        // 초기 회전값 저장
        originalRotation = transform.rotation;
        // 초기 각도 설정
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

        // 카메라 위치와 회전 계산
        Quaternion rotation;
        if (isTemporaryRotation)
        {
            rotation = Quaternion.Euler(currentY + tempY, currentX + tempX, 0);
        }
        else
        {
            rotation = Quaternion.Euler(currentY, currentX, 0);
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;

        //카메라가 물체에 닿으면 통과 되지 않게 하는 코드
        //플레이어와 카메라의 거리 구하기
        Vector3 rayDir = transform.position - target.position;
        //플레이어가 카메라 방향으로 쏘는 레이 발사
        if (Physics.Raycast(target.position, rayDir, out RaycastHit hit, distance, Ground)) 
        {
            //맞은 부위보다 더 안쪽으로 위치 이동
            transform.position = hit.point - rayDir.normalized * 0.1f;
        }

    }
}
