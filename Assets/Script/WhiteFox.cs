using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFox : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 시작하자마자 걷는 애니메이션 실행
        animator.Play("rigAction_001");
    }
}
