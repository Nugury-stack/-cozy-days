using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchOnStep : MonoBehaviour
{
    public Camera playerCamera;
    public Camera otherCamera;
    public GameObject playerObject; // 플레이어 전체 오브젝트
    public Transform returnPosition; // 플레이어 복귀용 위치
    public bool switchBackWhenExit = false;

    public GameObject rotatingObject; // 회전 대상 오브젝트
    public GameObject activateOnOtherCamera; // 예: 다리, 길 등

    private bool isSwitched = false;
    private bool isReturning = false;

    private RotateOnZ rotateScript; // 회전 스크립트 참조

    private void Start()
    {
        if (playerCamera != null) playerCamera.enabled = true;
        if (otherCamera != null) otherCamera.enabled = false;

        if (rotatingObject != null)
            rotateScript = rotatingObject.GetComponent<RotateOnZ>();

        if (activateOnOtherCamera != null)
            activateOnOtherCamera.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isSwitched && !isReturning)
        {
            StartCoroutine(SwitchBackWithDelay());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSwitched && other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 발판에 닿았습니다.");
            SwitchToOther();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (switchBackWhenExit && other.CompareTag("Player") && isSwitched)
        {
            Debug.Log("플레이어가 발판에서 나갔습니다. 플레이어 카메라로 복귀합니다.");
            StartCoroutine(SwitchBackWithDelay());
        }
    }

    private void SwitchToOther()
    {
        // 플레이어 비활성화
        if (playerObject != null)
            playerObject.SetActive(false);

        // 다른 카메라 활성화
        if (otherCamera != null)
        {
            otherCamera.gameObject.SetActive(true);
            otherCamera.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isSwitched = true;

        if (rotateScript != null)
            rotateScript.StartRotation();

        if (activateOnOtherCamera != null)
            activateOnOtherCamera.SetActive(true);
    }

    private IEnumerator SwitchBackWithDelay()
    {
        isReturning = true;
        yield return null; // 한 프레임 대기
        SwitchBackToPlayer();
        isReturning = false;
    }

    private void SwitchBackToPlayer()
    {
        if (playerObject != null)
        {
            // Rigidbody와 Collider 처리
            Rigidbody rb = playerObject.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // 위치 이동
            if (returnPosition != null)
            {
                playerObject.transform.position = returnPosition.position;
                playerObject.transform.rotation = returnPosition.rotation;
                Debug.Log($"플레이어 복귀 위치로 이동: {returnPosition.position}");
            }

            // 플레이어 다시 활성화
            playerObject.SetActive(true);

            if (rb != null) rb.isKinematic = false;
        }

        // 다른 카메라 비활성화
        if (otherCamera != null)
        {
            otherCamera.enabled = false;
            otherCamera.gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isSwitched = false;

        if (rotateScript != null)
            rotateScript.StopRotation();
    }
}
