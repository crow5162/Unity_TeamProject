using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    //사용하는 컴포넌트 선언 
    Rigidbody rigidbody;
    Animator animator;
    //몬스터의 상태 정의 
    public enum CurrentState {  Idle, Trace, Attack, Die}
    //몬스터 처음 상태 
    public CurrentState EnemyState = CurrentState.Idle;
    //몬스터 위치 저장 
    private Transform EnemyTr;





    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
