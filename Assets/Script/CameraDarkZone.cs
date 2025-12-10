using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraDarkZone : MonoBehaviour
{
    public Image darkOverlay;
    public float fadeSpeed = 2f;
    public GameObject cameraCanvas;
    public int requiredItemCount = 3; // 이 개수 이상이면 어두워지지 않음

    private float targetAlpha = 0f;

    void Start()
    {
        if (cameraCanvas != null)
        {
            cameraCanvas.SetActive(false);
        }
    }

    void Update()
    {
        Color color = darkOverlay.color;
        color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
        darkOverlay.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement itemCollector = other.GetComponent<PlayerMovement>();

            if (itemCollector != null && itemCollector.itemCount >= requiredItemCount)
            {
                // 아이템이 충분하면 어두워지지 않음
                targetAlpha = 0f;
            }
            else
            {
                // 아이템이 부족하면 어둡게
                targetAlpha = 1f;
            }

            if (cameraCanvas != null)
            {
                cameraCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetAlpha = 0f;

            if (cameraCanvas != null)
            {
                cameraCanvas.SetActive(false);
            }
        }
    }
}
