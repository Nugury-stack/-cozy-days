using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    public GameObject foxPrefab;             // 생성할 여우 프리팹
    public Transform[] spawnPoints;          // 미리 정해둔 위치들

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FoxSpawner: 플레이어 충돌 → 여우들을 정해진 위치에 생성!");

            foreach (Transform point in spawnPoints)
            {
                Instantiate(foxPrefab, point.position, point.rotation);
            }

            // 한 번만 작동하고 싶다면:
            gameObject.SetActive(false);
        }
    }
}

