using UnityEngine;
using System.Collections;

public class FloorDropper : MonoBehaviour
{
    public float fallSpeed = 5f; // 떨어지는 속도

    private bool isDropping = false;

    public void Drop(float delay)
    {
        StartCoroutine(DropAfterDelay(delay));
    }

    private IEnumerator DropAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isDropping = true;
    }

    void Update()
    {
        if (isDropping)
        {
            // Y값만 내려주기
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }
}
