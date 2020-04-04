using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //터렛 몸통 회전하는 부분 
    [SerializeField] Transform GunBody = null;
    //터렛의 사정거리 
    [SerializeField] float Range = 0f;
    //특정 레이어를 가진에만 공격할 수 있게 레이어 마스크 설정 
    [SerializeField] LayerMask layerMask = 0;

    //공격할 대상에게 트랜스 폼을 설정 해줌 
    Transform Target = null;

    //터렛의 사정거리에 있는 모든 컬라이더를 검출 
    void SearchPlayer()
    {
        //Collider[] coll = Physics.OverlapSphere(transform.position, Range, layerMask);
        ////터렛과 가장 가까운 오브젝트를 임시로 선언
        //Transform t_ShotestTarget = null;
        //
        ////생성된 컬라이더가 1개라도 있으면 실행될 수 있게 조건을 걸어준다 .
        //if (coll.Length > 0)
        //{
        //    //짧은 걸 비교하려면 가장 긴 녀석이 기준이되야함.
        //    float t_Shot testDistance = Mathf.Infinity;
        //    foreach (Collider colTarget in coll)
        //    {
        //        float Distance = Vector3.SqrMagnitude(transform.position - colTarget.transform.position);
        //        if (t_Shot testDistance > Distance)
        //        {
        //            t_Shot testDistance = Distance;
        //            t_Shot testTarget = colTarget.transform;
        //        }
        //    }
        //}

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
