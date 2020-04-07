using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //터렛 몸통 회전하는 부분 
    [SerializeField] Transform GunBody = null;
    //private Transform GunBody;
    //터렛의 사정거리 
    [SerializeField] float Range = 0f;
    //특정 레이어를 가진애만 공격할 수 있게 레이어 마스크 설정 
    [SerializeField] LayerMask layerMask = 0;
    //회전중에 타겟이 범위 안에 들어왔을 때 얼마나 빠른 속도로 회전해서 적에게 회전 할것인지 
    [SerializeField] float spinSpeed = 0f;
    //터렛의 연사 속도 변수
    [SerializeField] float fireRate = 0;
    //실제 연산에 쓸 변수 
    float currentFireRate;
    //총알 프리펩 
    public GameObject t_Bullet;
    //총알 발사 위치 
    public Transform firePos;


    
    //공격할 대상에게 트랜스 폼을 설정 해줌 // 최종 타겟  
    Transform Target = null;
    
    void SearchPlayer()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, Range, layerMask);
        //터렛과 가장 가까운 오브젝트를 임시로 선언
        Transform t_shortestTarget = null;
        if(t_cols.Length > 0)
        {
            float t_shortestDistance = Mathf.Infinity;
            foreach(Collider t_colTarget in t_cols)
            {
                // SqrMagnitude = 제곱 반환 (실제 거리 x 실제거리)
                // Distance = 루트 연산 후 반환 (실제 거리)
                float t_distance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position);
                if(t_shortestDistance > t_distance)
                {
                    t_shortestDistance = t_distance;
                    t_shortestTarget = t_colTarget.transform;
                }
            }
            
        }
        Target = t_shortestTarget;
    }

    void Start()
    { 
        // 프로그램이 시작과 동시에 연사변수의 연사속도를 넣어줌 
        currentFireRate = fireRate;
        //이 함수가 시작과 동시에 0.5초 마다 실행하도록 한다.
        InvokeRepeating("SearchPlayer", 0f, 0.5f);
    }

    void Update()
    {

        //만약 타겟이 없으면 포신은 계속해서 돈다 .
        if (Target == null)
        GunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
      
        else
        {
            Vector3 pos =  Target.position- transform.position;

            //Debug.Log(Target.position);

            //LookRotation - 특정 좌표를 바라보게 만드는 회전값을 리턴.           
            Quaternion t_lookRotation = Quaternion.LookRotation(pos.normalized);
            //Debug.Log("quater:" + t_lookRotation);

            //RotateTowards - a 지점에서 b 지점 까지 c의 스피드로 회전 
            //오일러값에서 y축만 반영되게 수정한 뒤 쿼티니온으로 변환 (Euler(오일러) angle은 x,y,z 3개의 축을 기준으로  0 ~ 360도 만큼 회전시키는 좌표계 
            Vector3 t_euler = Quaternion.RotateTowards(GunBody.rotation, t_lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            GunBody.rotation = Quaternion.Euler(0,t_euler.y,0);
            //Debug.Log(t_euler.y);

            //터렛이 조준해야 될 최종방향 
            Quaternion fireRotation = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);
            // 조준해야 될 각도차 
            if (Quaternion.Angle(GunBody.rotation, fireRotation) < 5f)
            {
                //1초에 1씩 감소하다가 0보다 작아지면 
                currentFireRate -= Time.deltaTime;
                if(currentFireRate <= 0)
                {
                    currentFireRate = fireRate;
                    Fire();
                }
            }

        }
    }

    void Fire()
    {
        GameObject Bullet = Instantiate(t_Bullet, firePos.position, firePos.rotation);
        Destroy(Bullet, 3.0f);
    }


}
