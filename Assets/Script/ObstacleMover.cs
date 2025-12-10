using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleMover : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 5f;

    private Rigidbody rb;
    private bool isMoving = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        Vector3 direction = targetPosition - rb.position;
        float distance = direction.magnitude;

        if (distance > 0.01f)
        {
            direction.Normalize();
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(targetPosition);
            isMoving = false;
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
