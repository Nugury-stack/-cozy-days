using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery_Movement : MonoBehaviour
{
    // === [인스펙터에서 설정 가능한 변수] ===

    [Header("상하 운동 설정")]
    [Tooltip("오브젝트가 이동할 중심 높이 (시작 Y 위치)")]
    public float centerY = 0f;

    [Tooltip("상하 운동의 진폭 (최대 이동 거리)")]
    public float amplitude = 1.0f;

    [Tooltip("상하 운동의 속도")]
    public float frequency = 1.0f; // 초당 몇 번 왕복할지


    [Header("회전 설정")]
    [Tooltip("Y축 회전 속도 (초당 도)")]
    public float rotationSpeedY = 50.0f; // 초당 50도 회전


    // === [내부 변수] ===

    private Vector3 startPosition;

    void Start()
    {
        // 오브젝트의 시작 위치를 저장합니다.
        startPosition = transform.position;
        // 인스펙터에서 설정된 중심 높이로 시작 위치의 Y 값을 재설정합니다.
        centerY = startPosition.y;
    }

    void Update()
    {
        // 1. 위아래 움직임 (Y축 진동)

        // Time.time은 게임 시작 후 경과된 시간입니다.
        // Mathf.Sin()은 -1과 1 사이의 값을 부드럽게 반복합니다.
        // 'frequency'를 곱하여 움직임 속도를 조절하고, 'amplitude'를 곱하여 이동 거리를 조절합니다.
        float newY = centerY + amplitude * Mathf.Sin(Time.time * frequency * 2 * Mathf.PI);

        // 오브젝트의 위치를 업데이트합니다. (X, Z는 그대로 유지)
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);


        // 2. Y축 회전 (오른쪽 회전)

        // Time.deltaTime은 마지막 프레임 이후 경과된 시간입니다.
        // 이것을 사용하면 프레임 속도에 관계없이 일정한 속도로 회전합니다.
        // Space.World는 월드 좌표계의 Y축을 기준으로 회전함을 의미합니다.
        transform.Rotate(Vector3.up * rotationSpeedY * Time.deltaTime, Space.World);
    }
}
