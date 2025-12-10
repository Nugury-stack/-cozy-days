using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrozenFollow : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (agent != null)
        {
            agent.speed = 40f;
            agent.updateRotation = false; // 회전 비활성화
        }

        /*
        agent.acceleration = 100f;
        agent.angularSpeed = 360f;
        agent.radius = 0.3f;
        agent.stoppingDistance = 0.5f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        */
    }

    void Update()
    {
        if (player == null) return;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.destination = player.position;
        }

        // 회전 고정: X = -90, Y = 0, Z = 0 (필요 시 Y/Z 조정 가능)
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 적과 충돌했습니다!");

            PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.FreezePlayer(2f); // 2초간 얼림
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
