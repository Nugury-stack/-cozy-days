using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_Door : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            Game_Manager.Instance.Stage_Clear();
            //다시 설정 창으로 이동 코드
        }
    }
}
