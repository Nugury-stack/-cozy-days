using UnityEngine;

public class RockAutoRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 0f, -200f); 

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
