using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFaller : MonoBehaviour
{
    public Transform treeVisual; // 회전시킬 실제 트리 모델
    public float fallDuration = 1.5f;
    public float fallAngle = 90f;
    public Vector3 fallAxis = Vector3.forward;

    private bool hasFallen = false;

    // 나무 쓰러뜨리기
    public void FallTree()
    {
        if (!hasFallen)
        {
            hasFallen = true;
            StartCoroutine(RotateTree());
        }
    }

    private IEnumerator RotateTree()
    {
        Quaternion startRotation = treeVisual.rotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(fallAngle, fallAxis);

        float elapsed = 0f;
        while (elapsed < fallDuration)
        {
            treeVisual.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / fallDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        treeVisual.rotation = endRotation;
    }
}
