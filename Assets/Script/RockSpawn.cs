using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab;
    public Vector3 areaSize = new Vector3(10f, 0f, 10f);
    public float spawnInterval = 3f;

    void Start()
    {
        Physics.gravity = new Vector3(0f, -50f, 0f); 
        InvokeRepeating(nameof(SpawnRock), 0f, spawnInterval); 
    }

    void SpawnRock()
    {
        
        float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randomZ = Random.Range(-areaSize.z / 2, areaSize.z / 2);
        Vector3 spawnPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        GameObject spawnedRock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

       
        OptimizeRock(spawnedRock);

        
        if (spawnedRock.GetComponent<RockAutoRotate>() == null)
        {
            spawnedRock.AddComponent<RockAutoRotate>();
        }

        
        Destroy(spawnedRock, 22f);
    }

    void OptimizeRock(GameObject rock)
    {
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        Collider col = rock.GetComponent<Collider>();

        if (rb != null)
        {
            rb.mass = 3f;
            rb.drag = 0f;
            rb.angularDrag = 0f; 
        }

        if (col != null)
        {
            PhysicMaterial noFriction = new PhysicMaterial
            {
                dynamicFriction = 0f,
                staticFriction = 0f,
                frictionCombine = PhysicMaterialCombine.Minimum
            };
            col.material = noFriction;
        }
    }
}
