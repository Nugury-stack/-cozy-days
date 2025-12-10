using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAme_Exit : MonoBehaviour
{
    //Exit_Game
    // 전처리기 명령어를 이용해 에디터에서 그리고 에디터가 아닐때 어느 환경이든 실행 종료 시키는 코드
    public void Game_Exit()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stone"))
        {
            Game_Exit();
        }
    }
}
