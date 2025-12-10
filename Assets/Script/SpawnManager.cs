using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [Header("스폰 범위 크기 (빈 오브젝트 중심)")]
    public Vector3 areaSize = new Vector3(10f, 0f, 10f);

    [Header("스폰 설정")]
    public GameObject[] spawnPrefabs;
    public float spawnInterval = 2f;
    public int spawnCountPerInterval = 2;
    public int maxSpawnCount = 10;

    [Header("플레이어 감지")]
    public Transform player;

    private bool playerInZone = false;
    private Coroutine spawnCoroutine;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private Vector3 areaMin;
    private Vector3 areaMax;

    void Start()
    {
        Vector3 center = transform.position;
        areaMin = center - areaSize / 2f;
        areaMax = center + areaSize / 2f;
    }

    void Update()
    {
        if (player == null) return;

        bool isInZone = IsPlayerInsideArea();

        if (isInZone && !playerInZone)
        {
            playerInZone = true;
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
        else if (!isInZone && playerInZone)
        {
            playerInZone = false;
            StopCoroutine(spawnCoroutine);
            ClearAllMonsters();
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < spawnCountPerInterval; i++)
            {
                if (spawnedMonsters.Count >= maxSpawnCount)
                    break;

                GameObject monster = SpawnRandomPrefab();
                if (monster != null)
                    spawnedMonsters.Add(monster);
            }
        }
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;

        for (int attempt = 0; attempt < 20; attempt++)
        {
            float randomX = Random.Range(areaMin.x, areaMax.x);
            float randomZ = Random.Range(areaMin.z, areaMax.z);

            Vector3 rayOrigin = new Vector3(randomX, basePosition.y + 50f, randomZ);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
            {
                Vector3 groundPos = hit.point;

                if (NavMesh.SamplePosition(groundPos, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                {
                    if (navHit.position.x >= areaMin.x && navHit.position.x <= areaMax.x &&
                        navHit.position.z >= areaMin.z && navHit.position.z <= areaMax.z)
                    {
                        return navHit.position;
                    }
                }
            }
        }

        if (NavMesh.SamplePosition(basePosition, out NavMeshHit centerHit, 20f, NavMesh.AllAreas))
        {
            return centerHit.position;
        }

        Debug.LogWarning("NavMesh 위의 랜덤 위치를 찾지 못했습니다. 스폰 존 중앙 위치 반환");
        return basePosition;
    }

    public GameObject SpawnRandomPrefab()
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
            return null;

        int idx = Random.Range(0, spawnPrefabs.Length);
        Vector3 pos = GetRandomPosition();
        return Instantiate(spawnPrefabs[idx], pos, Quaternion.identity);
    }

    private void ClearAllMonsters()
    {
        foreach (GameObject monster in spawnedMonsters)
        {
            if (monster != null)
                Destroy(monster);
        }
        spawnedMonsters.Clear();
    }

    private bool IsPlayerInsideArea()
    {
        Vector3 pos = player.position;
        return pos.x >= areaMin.x && pos.x <= areaMax.x &&
               pos.z >= areaMin.z && pos.z <= areaMax.z;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Vector3 center = transform.position;
        Vector3 size = areaSize;
        Gizmos.DrawCube(center, size);
    }

    public void UnregisterMonster(GameObject monster)
    {
        if (spawnedMonsters.Contains(monster))
        {
            spawnedMonsters.Remove(monster);
        }
    }
}
