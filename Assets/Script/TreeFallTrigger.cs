using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFallTrigger : MonoBehaviour
{
    public List<TreeFaller> trees;              // 쓰러뜨릴 나무들
    public float delayBetweenTrees = 1.0f;      // 나무마다 쓰러지는 간격 (초)
    private bool hasTriggered = false;          // 한 번만 작동하게 제어

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(FallTreesSequentially());
        }
    }

    private IEnumerator FallTreesSequentially()
    {
        foreach (var tree in trees)
        {
            if (tree != null)
                tree.FallTree(); // 나무 쓰러뜨리기

            yield return new WaitForSeconds(delayBetweenTrees); // 다음 나무까지 대기
        }
    }
}
