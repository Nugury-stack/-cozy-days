using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    public ObstacleMover[] obstacles;   // 동시에 작동할 장애물들

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 트리거 존에 닿음");

            foreach (var obstacle in obstacles)
            {
                Debug.Log("장애물 작동 시도: " + obstacle?.name);
                obstacle?.StartMoving(); // 장애물 이동 시작
            }
        }
    }
}
