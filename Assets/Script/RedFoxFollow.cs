using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedFoxFollow : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        // 민첩하게 반응하도록 설정
        
        agent.acceleration = 100f;
        agent.angularSpeed = 360f;
        agent.stoppingDistance = 0.1f;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        //  매 프레임, 정확히 플레이어의 현재 위치로 계속 갱신
        agent.destination = player.position;

        //  플레이어를 향해 회전
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        //  애니메이션 적용
        animator.SetBool("isMoving", agent.velocity.magnitude > 0.1f);
    }
}

