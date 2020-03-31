using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//네비 기능을 사용하기 위해서는 추가야해야하는 네임스페이스 <중요>
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Enemymove : MonoBehaviour
{
    //순찰 지점들을 저장하기 위한 리스트 타입의 변수 
    public List<Transform> wayPoints;
    // 다음 순찰 지점 배열의 Index
    public int nextIdx;
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 5.0f;

    //회전 할 때 속도 
    private float damping = 1.0f;
    //navMeshAgent 컴포넌트를 저장할 변수 
    private NavMeshAgent agent;
    //적 캐릭터의 Transform 컴포넌트를 저장할 변수 
    private Transform enemyTr;
    //순찰 여부를 판단하는 변수 
    private bool _patrolling;
    //patrolling 프로퍼티 정의 (Getter, Setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if(_patrolling)
            {
                agent.speed = patrolSpeed;
                //순찰 상태 회전
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }
   
    //추적 대상의 위치를 저장하는 변수 
    private Vector3 _traceTarget;
    //traceTarget 프로퍼티 정의 
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            // 추적 상태 일 때 회전 계수
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    // navmesh의 이동속도에 대한 프로퍼티 정의 (getter)
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }


    void Start()
    {
        //적 캐릭터의 transform 컴포넌트를 추출 후 변수에 저장 한다!
        enemyTr = GetComponent<Transform>();

        // NavMeshAgent 컴포넌트를 추출한 후 변수에 저장 
        agent = GetComponent<NavMeshAgent>();
        // 목적지에 가까워 질수록 속도를 줄이는 옵션을 비활성화 
        agent.autoBraking = false;  //  목적지에 가까워 질 때마다 속도를 줄이는구나 <생각하기>
        //자동으로 회전하는 기능을 비활성화
        agent.updateRotation = false;

        agent.speed = patrolSpeed;

        // 계층 창의 WayPointBox 추출 순찰 지점 
        var Box = GameObject.Find("WaypointBox");
        //만약 Box 의 값이 null 값이 아니라면 
        if(Box != null)
        {
            // WayPointBox 하위에 있는 모든 Transform 컴포넌트를 추출한 후 List타입의 wayPoint의 배열에 추가 
            Box.GetComponentsInChildren<Transform>(wayPoints);
            //배열의 첫 번째 항목을 삭제  
            //삭제를 하지않으면 페어런트인 게임오브젝트 ( WayPointBox)도 순찰지점으로 들어감. 
            //참고 하기
            wayPoints.RemoveAt(0);
        }

        //MoveWayPoint();
        this.patrolling = true;
    }
    // 다음 목적지 까지 이동 명령을 줄꺼임. 
    void MoveWayPoint()
    {
        //최단 거리 경고 계산이 끝나지 않았으면 다음을 수행 하지 않음.
        if (agent.isPathStale) return;
        {
            // 다음 목적지를 waypoint 배열에서 추출한 위치로 다음  목적지를 지정 
            agent.destination = wayPoints[nextIdx].position;
            // 네비 기능을 활성화해서 이동을 시작함
            agent.isStopped = false;
        }


    }
    // 주인공을 추적할 때 이동시키는 함수 
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
    //순찰 및 추적을 정지 시키는 함수 
    public void Stop()
    {
        agent.isStopped = true;
        //바로 정지하기 위해 속도를  0으로 설정 
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Update()
    {
        //적 캐릭터가 이동 중일 때만 회전 
        if (agent.isStopped == false)
        {
            //NavMeshAgent가가야할 방향 벡터를 쿼터니언 타입의 각도로 변환 
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //보간 함수를 사용해 점진적으로 회전 시킴 
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (!_patrolling) return;

        //NavmeshAgent 가 이동하고 있고 목적지에 도착했는지 여부를 계산 
        if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {
            //다음 목적지의 배열 첨자를 게산
            nextIdx = ++nextIdx % wayPoints.Count;
            // 다음 목적지로 이동 명령을 수행
            MoveWayPoint();
        }
    }
}
