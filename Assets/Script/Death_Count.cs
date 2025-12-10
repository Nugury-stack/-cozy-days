using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Death_Count : MonoBehaviour
{
    public static Death_Count Instance;

    public TextMeshProUGUI Death_Cnt;
    public static int Death = 0;

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

    public void Death_C() 
    {
        Death += 1;
        Death_Cnt.text = Death.ToString("D3"); 
    }
}
