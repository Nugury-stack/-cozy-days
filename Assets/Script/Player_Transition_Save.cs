using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player_Transition_Save : MonoBehaviour
{
    public static Player_Transition_Save Instance;
    public GameObject player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void FindPlayer()
    {
       player = GameObject.Find("Human파자마");
    }

    public void savePosition()//설정로비 가기전 스테이지 위치 값저장
    {
        FindPlayer();
        this.transform.position = player.transform.position;
    }

    public void restorePosition()//로비에서 스테이지로 갈때 플레이어 위치 이동
    {
        FindPlayer();
        player.transform.position = this.transform.position;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        if (scene.name == "Stage_1") 
        {
            StartCoroutine(DelayedRestore());
        }
    }

    private IEnumerator DelayedRestore()
    {
        yield return new WaitForSeconds(0.1f); // 약간의 프레임 딜레이
        restorePosition();
    }
}
