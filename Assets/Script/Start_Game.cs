using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Start_Game : MonoBehaviour
{
    public static Start_Game Instance;

    void Awake()
    {       
        Instance = this;
    }
    public void Game_Start(int stageIndex)
    {
        SceneManager.LoadScene(stageIndex);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stone"))
        {
            Game_Start(Game_Manager.Instance.stageIndex);
        }
    }
}
