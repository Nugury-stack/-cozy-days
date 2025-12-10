using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public float upHeight = 5f;        // 트랩이 올라갈 높이
    public float moveSpeed = 3f;       // 트랩이 올라가는 속도
    public float delayTime = 10f;      // 발판 밟은 후 올라가기까지 대기 시간
    public float stayTime = 5f;        // 위에서 머무는 시간

    private bool isRunning = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.up * upHeight;
    }

    public void ActivateTrap()
    {
        if (!isRunning)
        {
            StartCoroutine(TrapSequence());
        }
    }

    private IEnumerator TrapSequence()
    {
        isRunning = true;

        // 대기 시간 후 트랩 상승
        yield return new WaitForSeconds(delayTime);
        yield return StartCoroutine(MoveTrap(originalPosition, targetPosition));

        // 일정 시간 머무름
        yield return new WaitForSeconds(stayTime);

        // 다시 내려감
        yield return StartCoroutine(MoveTrap(targetPosition, originalPosition));

        isRunning = false;
    }

    private IEnumerator MoveTrap(Vector3 start, Vector3 end)
    {
        float elapsed = 0f;
        float duration = Vector3.Distance(start, end) / moveSpeed;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }

    
}
