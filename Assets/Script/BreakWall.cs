using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    public GameObject[] fragments;

    private bool isBroken = false;

    private void Start()
    {
        foreach (GameObject frag in fragments)
        {
            // 초기 Rigidbody 준비
            Rigidbody rb = frag.GetComponent<Rigidbody>();
            if (rb == null)
                rb = frag.AddComponent<Rigidbody>();

            rb.isKinematic = true; // 시작은 고정

            // Collider 없으면 추가
            Collider col = frag.GetComponent<Collider>();
            if (col == null)
                col = frag.AddComponent<MeshCollider>();

            // MeshCollider일 경우 convex 체크
            if (col is MeshCollider meshCol)
                meshCol.convex = true;

            frag.SetActive(true); // 항상 활성화 상태 유지
        }
    }

    public void Break()
    {
        if (isBroken) return;
        isBroken = true;

        foreach (GameObject frag in fragments)
        {
            Collider col = this.GetComponent<Collider>();
            col.isTrigger = true; // 충돌 무시

            Rigidbody rb = frag.GetComponent<Rigidbody>();
            rb.isKinematic = false; // 물리 작동 켜기
            rb.useGravity = true;

            rb.AddExplosionForce(700f, transform.position, 5f);
        }

        // 5초 후에 전체 파괴
        Destroy(gameObject, 2.4f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            Debug.Log("돌이 벽에 충돌했습니다!");
            Break();
            Destroy(collision.gameObject); // 돌 제거
        }
    }
}
