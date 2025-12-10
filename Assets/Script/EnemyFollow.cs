using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [SerializeField] private Transform targetPoint;  // 몬스터 머리 빈 오브젝트
    [SerializeField] private float rotationSpeed = 5f; // 부드럽고 빠른 회전 속도

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (agent != null)
        {
            agent.speed = 40f;
            agent.updateRotation = true;  // 몬스터 몸은 NavMeshAgent가 회전 관리
        }

        if (targetPoint == null)
        {
            Debug.LogError("targetPoint(머리 빈 오브젝트)가 할당되지 않았습니다!");
        }
    }

    void Update()
    {
        if (player == null || targetPoint == null) return;

        // NavMeshAgent가 플레이어 위치로 이동하게 설정
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }

        // 머리 빈 오브젝트가 몬스터 위치(몸)를 따라오도록 위치 고정
        targetPoint.position = transform.position + Vector3.up * 1.5f;  // 높이 조정 (머리 높이)

        // 머리가 플레이어 방향을 바라보도록 회전
        Vector3 directionToPlayer = player.position - targetPoint.position;
        directionToPlayer.y = 0f;  // 수평으로만 회전

        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            targetPoint.rotation = Quaternion.Slerp(targetPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 적과 충돌했습니다!");

            PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.ApplySlow(2f, 4f);
            }

            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.UnregisterMonster(gameObject);
            }

            Destroy(gameObject);
        }
    }
}
