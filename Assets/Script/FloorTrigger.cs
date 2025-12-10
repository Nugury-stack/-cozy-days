using UnityEngine;
using System.Collections;

public class FloorTrigger : MonoBehaviour
{
    public FloorDropper[] floors;        // 떨어질 바닥들
    public float floorDropInterval = 1f; // 순차적으로 떨어지는 시간 간격
    public float triggerDelay = 5f;      // 트리거 발동 후 처음 기다릴 시간

    private bool hasTriggered = false;   // 중복 발동 방지용

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("플레이어가 바닥 트리거 존에 닿음 -> " + triggerDelay + "초 후 발동 예정");

            StartCoroutine(ActivateFloorDrop());
        }
    }

    private IEnumerator ActivateFloorDrop()
    {
        // 트리거 발동 후 5초 기다림
        yield return new WaitForSeconds(triggerDelay);

        // 바닥 순차 낙하 시작
        for (int i = 0; i < floors.Length; i++)
        {
            if (floors[i] != null)
            {
                float delay = i * floorDropInterval;
                floors[i].Drop(delay);
            }
        }
    }
}
