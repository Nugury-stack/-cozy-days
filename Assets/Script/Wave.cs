using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Wave : MonoBehaviour
{
    Mesh mesh;
    Vector3[] baseVertices;
    Vector3[] displacedVertices;

    public float waveHeight = 0.5f;
    public float waveScale = 1f;
    public float waveSpeed = 1f;

    void Start()
    {
        // Mesh 복사본 생성
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = Instantiate(mf.sharedMesh);
        mf.mesh = mesh;

        baseVertices = mesh.vertices;
        displacedVertices = new Vector3[baseVertices.Length];
    }

    void Update()
    {
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];

            float noise = Mathf.PerlinNoise(
                (vertex.x * waveScale) + (Time.time * waveSpeed),
                (vertex.z * waveScale) + (Time.time * waveSpeed)
            );

            vertex.y = (noise - 0.5f) * 2f * waveHeight;
            displacedVertices[i] = vertex;
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }
}
