using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snowman : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private float updateTimer = 0f;


    [SerializeField] private float rotateSpeed = 120f; // 회전 속도 (도/초)

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (!NavMesh.SamplePosition(transform.position, out _, 20f, NavMesh.AllAreas))
        {
            Debug.LogError("몬스터가 NavMesh 위에 있지 않습니다!");
        }

        if (agent != null)
        {
            agent.speed = 20f;
            agent.updateRotation = false; // 수동 회전
        }
        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, 100f, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
        else
        {
            Debug.LogWarning("플레이어 위치가 NavMesh 위에 없습니다!");
        }
    }

    void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer > 0.3f)
        {
            updateTimer = 0f;
            if (agent.isOnNavMesh)
                agent.SetDestination(player.position);
        }
        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾지 못함");
            return;
        }

        Debug.Log($"agent.destination: {agent.destination}, hasPath: {agent.hasPath}, pathPending: {agent.pathPending}, remainingDistance: {agent.remainingDistance}");

        agent.destination = player.position;

        if (!agent.hasPath && !agent.pathPending)
        {
            Debug.LogWarning("경로 없음! agent가 움직일 수 없음.");
        }

        //  Y축으로 계속 회전
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
