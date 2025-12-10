using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotateTrap : MonoBehaviour
{
    public float rotationSpeed = 90f; // 초당 회전 속도

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 유지
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(0f, 0f, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}