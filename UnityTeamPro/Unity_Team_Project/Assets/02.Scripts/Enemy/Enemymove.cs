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

    //navMeshAgent 컴포넌트를 저장할 변수 
    private NavMeshAgent agent;


    void Start()
    {
        // NavMeshAgent 컴포넌트를 추출한 후 변수에 저장 
        agent = GetComponent<NavMeshAgent>();
        // 목적지에 가까워 질수록 속도를 줄이는 옵션을 비활성화 
        agent.autoBraking = false;  //  목적지에 가까워 질 때마다 속도를 줄이는구나 <생각하기>

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

        MoveWayPoint();
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


    void Update()
    {
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
