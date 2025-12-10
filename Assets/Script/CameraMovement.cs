using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // 카메라가 따라갈 객체 (플레이어나 다른 객체)
    public Transform objectTofollow;

    // 카메라가 객체를 따라가는 속도
    public float followSpeed = 10f;

    // 마우스 이동 민감도
    public float sensitivity = 300f;

    // 카메라의 상하 회전 제한 각도
    public float clampAngle = 70f;

    // 카메라의 상하 회전 각도 (초기값)
    private float rotX;

    // 카메라의 좌우 회전 각도 (초기값)
    private float rotY;

    // 실제 카메라의 Transform (카메라 위치를 조정)
    public Transform realCamera;

    // 카메라가 이동할 방향 벡터 (정규화된 값)
    public Vector3 dirNormalized;

    // 카메라의 목표 위치 (최종적으로 설정된 방향)
    public Vector3 finalDir;

    // 카메라와 객체 간의 최소 거리
    public float minDistance;

    // 카메라와 객체 간의 최대 거리
    public float maxDistance;

    // 실제 카메라의 거리 (최종 거리)
    public float finalDistance;

    // 카메라 위치 보간 시 부드럽게 만드는 정도
    public float smoothness = 10f;

    
    void Start()
    {
        // 카메라의 초기 상하 회전 값 (x축 회전)
        rotX = transform.localRotation.eulerAngles.x;

        // 카메라의 초기 좌우 회전 값 (y축 회전)
        rotY = transform.localRotation.eulerAngles.y;

        // 실제 카메라의 위치를 정규화하여 방향 벡터로 설정
        dirNormalized = realCamera.localPosition.normalized;

        // 카메라와 객체 간의 초기 거리 설정
        finalDistance = realCamera.localPosition.magnitude;

        // 커서를 화면 중앙에 고정하고, 보이지 않게 설정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        // 마우스 Y축 이동에 따른 카메라 상하 회전 처리
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;

        // 마우스 X축 이동에 따른 카메라 좌우 회전 처리
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // 상하 회전 값 제한 (최대 및 최소 각도 설정)
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        // 회전 값에 맞춰 카메라의 회전 적용 (Quaternion으로 변환)
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot; // 카메라 회전 적용
    }

    
    private void LateUpdate()
    {
        // 카메라가 objectToFollow의 위치로 이동 (followSpeed에 따라 속도 조정)
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);

        // 카메라의 목표 위치 계산 (카메라의 방향으로 maxDistance 만큼 떨어진 위치)
        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        
        RaycastHit hit;

        // 카메라와 목표 위치 사이에 장애물이 있는지 확인 (Linecast)
        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            // 장애물이 있을 경우, 장애물과의 거리를 계산하여 finalDistance를 설정
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            // 장애물이 없으면 최대로 설정된 거리로 카메라 위치 설정
            finalDistance = maxDistance;
        }

        // 실제 카메라의 위치를 부드럽게 보간하여 이동 (Lerp를 사용)
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }
}
