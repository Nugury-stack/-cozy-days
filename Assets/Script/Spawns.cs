using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Spawns : MonoBehaviour
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
        // 빈 오브젝트 위치를 중심으로 areaMin, areaMax 계산
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
            Debug.Log("플레이어가 스폰 존에 들어왔습니다.");
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
        else if (!isInZone && playerInZone)
        {
            Debug.Log("스폰 존에서 나감. 스폰 중단");
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
                {

                    break;
                }

                GameObject monster = SpawnRandomPrefab();
                if (monster != null)
                {
                    spawnedMonsters.Add(monster);
                }
                else
                {
                    Debug.LogWarning("SpawnRandomPrefab() 호출 결과가 null이므로 몬스터 생성 실패");
                }
            }
        }
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position;

        if (NavMesh.SamplePosition(center, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            Debug.Log($"스폰 위치: {hit.position}");
            return hit.position;
        }

        Debug.LogError("스폰 중심 근처에 NavMesh가 없습니다. 몬스터 생성 취소.");
        return Vector3.zero; // 위치 찾기 실패
    }


    public GameObject SpawnRandomPrefab()
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0)
        {
            Debug.LogWarning("SpawnPrefabs가 비어있음!");
            return null;
        }

        int idx = Random.Range(0, spawnPrefabs.Length);
        Vector3 pos = GetRandomPosition();

        if (pos == Vector3.zero)
        {
            return null; // NavMesh 위치 못 찾았으면 생성하지 않음
        }

        return Instantiate(spawnPrefabs[idx], pos, Quaternion.identity);
    }

    private void ClearAllMonsters()
    {
        foreach (GameObject monster in spawnedMonsters)
        {
            if (monster != null)
            {
                Destroy(monster);
            }
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