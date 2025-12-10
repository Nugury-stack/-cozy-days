using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxPusher : MonoBehaviour
{
    public float pushDistance = 3f;
    public float pushDuration = 0.2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PushPlayerOverTime(collision.gameObject));
        }
    }

    private IEnumerator PushPlayerOverTime(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null) yield break;

        Vector3 start = rb.position;
        Vector3 direction = (start - transform.position).normalized;
        direction.y = 0f;

        Vector3 end = start + direction * pushDistance;
        float elapsed = 0f;

        while (elapsed < pushDuration)
        {
            float t = elapsed / pushDuration;
            Vector3 targetPosition = Vector3.Lerp(start, end, t);
            rb.MovePosition(targetPosition);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate(); // 물리 프레임 기준
        }

        rb.MovePosition(end);
    }
}