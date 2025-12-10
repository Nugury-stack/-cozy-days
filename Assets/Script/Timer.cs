using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public TextMeshProUGUI timerText;
    private bool isPaused = false;  // 시간을 멈추는 변수
    public float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        timer = Time.realtimeSinceStartup;
        StartCoroutine(TimerCoroution());
    }

    IEnumerator TimerCoroution()
    {
        if (!isPaused)
        {
            timer += 1f; // 매초 1초씩 증가
        }

        int hours = (int)(timer / 3600);
        int minutes = (int)(timer / 60 % 60);
        int seconds = (int)(timer % 60);

        timerText.text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");

        yield return new WaitForSeconds(1f);
        StartCoroutine(TimerCoroution());
    }
    
    public void PauseTimer()
    {
        isPaused = true;
        Time.timeScale = 0f;//시간정지
    }
}