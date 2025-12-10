using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnZ : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private bool isRotating = false;

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }
}
