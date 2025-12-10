using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;
    public int stageIndex = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // gameObject는 이 스크립트가 붙은 객체 자신만 의미
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    //스테이지 클리어시 다음 스테이지로만 갈수 있게
    public void Stage_Clear()
    {
        this.stageIndex = this.stageIndex + 1;
    }

    // Game_Setting_함수
    public void Game_Setting()
    { 
        
    }

    // view_Credit_함수
    public void view_Credit()
    { 
        
    }
}
