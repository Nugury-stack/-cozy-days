using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // 회전 속도 조절 변수
    public Material skyboxMaterial; // 스카이박스 머티리얼 변수

    void Start()
    {
        // 스카이박스 머티리얼을 가져옵니다. (씬에 직접 적용된 경우)
        if (skyboxMaterial == null)
        {
            skyboxMaterial = RenderSettings.skybox;
        }

        // 스카이박스 머티리얼이 없으면 에러 메시지를 출력합니다.
        if (skyboxMaterial == null)
        {
            Debug.LogError("Skybox material is not assigned or found.");
            enabled = false; // 스크립트 비활성화
        }
    }

    void Update()
    {
        // 시간에 따라 Y축 회전 값을 증가시킵니다.
        float rotation = Time.time * rotationSpeed;
        // 스카이박스 머티리얼의 Rotation 값을 변경합니다.
        skyboxMaterial.SetFloat("_Rotation", rotation);
    }
}