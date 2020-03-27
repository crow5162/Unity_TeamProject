using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy에 관한곳 
public class Enemy : MonoBehaviour
{
    //사용하는 컴포넌트 선언 
    Rigidbody rigidbody;
    Animator animator;
    //몬스터의 상태 정의 
    public enum CurrentState {  Idle, Trace, Attack, Die}
    //몬스터 처음 상태 
    public CurrentState EnemyState = CurrentState.Idle;
    //플레이어 위치 저장
    private Transform PlayerTr;
    
    //몬스터 위치 저장 
    private Transform EnemyTr;

    //공격 사정 거리 
    public float AttackRange = 7.0f;
    //추적 사정 거리 
    public float TraceRange = 15.0f;

    // 사망 여부를 판단
    public bool IsDie = false;

    //코루틴 사용할 때 지연시간을 사용할 변수 
    private WaitForSeconds cTime;
    //이동을 제어하는 EnemyMove 클래스를 저장할 변수 
    private Enemymove enemymove;



    //한번만 호출되는 함수 
    void Awake()
    {
        //주인공의 게임 오브젝트를 추출 
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        //주인공의 Transform 컴포넌트 추출 
        if(player != null)
        {
            PlayerTr = player.GetComponent<Transform>();
        }
        //적 캐릭터의 Transform 컴포넌트 추출
        EnemyTr = GetComponent<Transform>();
        //이동을 제어하는 enemyMove 클래스 추출 
        enemymove = GetComponent<Enemymove>();
        cTime = new WaitForSeconds(0.3f);
    }

    // 활성화가 될 때마다 호출되는 함수 (중요! 활성화 될 때!!!! 마다)
    void OnEnable()
    {
        //CheckState코루틴 함수 실행
        StartCoroutine(CheckState());
        //Action 코루틴 함수를 실행
        StartCoroutine(Action());
    }
    // 에너미의 상태를 검사하는 코루틴함수 .
    IEnumerator CheckState()
    {
        //적 에너미가 사망하기 전까지 도는 무한 루트 생성
        while (!IsDie)
        {
            // 현재 상태가 사망이면 이 코루틴 함수를 종료
            if (EnemyState == CurrentState.Die) yield break;

            // 주인공과 적 캐릭터 간의 거리를 계산  
            // range라고 플레이어와 몬그터의 거리를 두고  비고할꺼임.
            float range = Vector3.Distance(PlayerTr.position, EnemyTr.position);

            //공격 사정거리에 들어온  경우 
            if(range <= AttackRange)
            {
                EnemyState = CurrentState.Attack;
            }
            // 추적 거리  들어 왔을 때 
            else if (range <= TraceRange)
            {
                EnemyState = CurrentState.Trace;
            }
            // 그 외 아무것도 아닐 때 아이들 상태로 
            else
            {
                EnemyState = CurrentState.Idle;
            }
            // 코루틴 함수는 0.3초 간격으로 확인을 하기 때문에 
            yield return cTime;

        }
    }

    IEnumerator Action()
    {
        // 적 캐릭터가 죽을 때 까지 무한 루프 스타트
        while (!IsDie)
        {
            yield return cTime;

            switch (EnemyState)
            {
                case CurrentState.Idle:
                    break;
                case CurrentState.Trace:
                    break;
                case CurrentState.Attack:
                    break;
                case CurrentState.Die:
                    break;
            }
        }
    }


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        
    }
}
