using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public PathRevealer pathRevealer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            Debug.Log("돌이 타겟에 충돌했습니다!");
            pathRevealer.RevealPath();
            Destroy(collision.gameObject);  // 돌 삭제 (선택)
        }
    }
}
