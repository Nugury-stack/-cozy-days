using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStartScene : MonoBehaviour
{
    // 특정 씬으로 이동하는 함수
    public void LoadScene(string sceneName)
    {
        // 입력받은 sceneName에 해당하는 씬으로 이동
        SceneManager.LoadScene(sceneName);
    }
}
