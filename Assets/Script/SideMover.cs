using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMover : MonoBehaviour
{
    public float moveSpeed = 2f;       // 움직이는 속도
    public float moveDistance = 3f;    // 좌우 이동 범위

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;     // 초기 위치 저장 (월드 좌표 기준)
        startRot = transform.rotation;     // 초기 회전 저장
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;

        // 오브젝트의 초기 로컬 X축 방향을 기준으로 offset만큼 이동
        Vector3 direction = startRot * Vector3.right; // 로컬 X축을 월드 방향으로 변환
        transform.position = startPos + direction * offset;
    }
}
