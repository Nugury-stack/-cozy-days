using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public List<TrapController> trapControllers; // 여러 대나무 함정들을 연결
    private MeshRenderer meshRenderer;

    void Start()
    {
        // MeshRenderer를 비활성화하여 발판이 보이지 않도록 설정
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // 발판을 보이지 않게 설정
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 플레이트에 닿음");
            foreach (var trap in trapControllers)
            {
                Debug.Log("트랩 작동 시도");
                trap?.ActivateTrap(); // 트랩을 활성화
            }
        }
    }
}
