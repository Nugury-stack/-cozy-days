using UnityEngine;

public class MaceSwing : MonoBehaviour
{
    public float swingSpeed = 2f;
    public float swingAngle = 45f;

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.rotation = startRotation * Quaternion.Euler(0f, 0f, angle); 
    }
}