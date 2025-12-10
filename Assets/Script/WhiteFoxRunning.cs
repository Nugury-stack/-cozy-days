using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFoxRunning : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;

        // ��ø�ϰ� �����ϵ��� ����

        agent.acceleration = 100f;
        agent.angularSpeed = 360f;
        agent.stoppingDistance = 0.1f;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        //  �� ������, ��Ȯ�� �÷��̾��� ���� ��ġ�� ��� ����
        agent.destination = player.position;

        //  �÷��̾ ���� ȸ��
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        //  �ִϸ��̼� ����
        animator.SetBool("isMoving", agent.velocity.magnitude > 0.1f);
    }
}
