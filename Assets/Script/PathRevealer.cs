using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRevealer : MonoBehaviour
{
    public GameObject[] hiddenPaths;
    public float visibleDuration = 5f;

    private void Start()
    {
        // 시작할 때 모든 숨겨진 길을 안 보이게 설정
        foreach (GameObject path in hiddenPaths)
        {
            MeshRenderer renderer = path.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = false;
        }

        Debug.Log("숨겨진 길이 초기화되었습니다. 시작 시 보이지 않음.");
    }

    public void RevealPath()
    {
        StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        foreach (GameObject path in hiddenPaths)
        {
            MeshRenderer renderer = path.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = true;
        }

        Debug.Log($"숨겨진 길이 {visibleDuration}초 동안 나타났습니다!");

        yield return new WaitForSeconds(visibleDuration);

        foreach (GameObject path in hiddenPaths)
        {
            MeshRenderer renderer = path.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = false;
        }

        Debug.Log("숨겨진 길이 다시 사라졌습니다.");
    }
}

