using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class MixerVolumeTrigger : MonoBehaviour
{
    [Header("오디오 믹서")]
    public AudioMixer mixer;

    [Header("제어할 그룹")]
    public AudioMixerGroup targetGroup; // BGM이나 SFX 그룹 드래그

    [Header("볼륨 변경 설정")]
    public float dBStep = -1f;

    private string exposedParam; // 내부적으로 자동 추출
    private HashSet<GameObject> triggered = new HashSet<GameObject>();

    void Start()
    {
        if (targetGroup != null)
        {
            exposedParam = targetGroup.name + "Volume"; // 관례적으로 이름 짐작
            float test;
            if (!mixer.GetFloat(exposedParam, out test))
            {
                Debug.LogError($" {exposedParam} 이란 이름의 파라미터를 찾을 수 없음. AudioMixer에서 확인하세요.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if (triggered.Contains(other.gameObject)) return;

        triggered.Add(other.gameObject);

        if (mixer.GetFloat(exposedParam, out float currentVolume))
        {
            float newVolume = Mathf.Clamp(currentVolume + dBStep, -80f, 0f);
            mixer.SetFloat(exposedParam, newVolume);
            Debug.Log($"[{other.name}] 충돌로 {exposedParam} 변경: {currentVolume:F1} → {newVolume:F1}");
        }
        else
        {
            Debug.LogWarning($" Mixer 파라미터 '{exposedParam}'을 가져올 수 없습니다.");
        }
    }
}
