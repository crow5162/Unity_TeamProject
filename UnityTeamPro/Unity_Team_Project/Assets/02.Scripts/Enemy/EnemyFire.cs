using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    //AudioSource 
    private AudioSource audio;
    //Animator
    private Animator animator;
    //Player Transform

    private Transform playerTr;
    //Enemy Transform
    private Transform enemyTr;
    //Animator Controler  Fire Hash 추출
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");
    //next Fire Time 
    private float nextFire = 0.0f;
    //Fire Time
    private readonly float fireRate = 0.1f;
    //주인공 향해 회전할 속도 
    private readonly float damping = 20.0f;
    //재장전 시간
    private readonly float reloadTime = 3.0f;
    //탄창의 최대 총알 수 
    private readonly int maxBullet = 5;
    //초기 총알 수 
    private int currBullet = 5;
    //재장전 여부 
    private bool isReload = false;
    //재장전 시간 동안 기다릴 변수 선언
    private WaitForSeconds wsReload;

    // 총 발사 여부
    public bool isFire = false;
    
    // 총 사운드 
    public AudioClip fireSfx;
    // 재장전 사운드를 저장할 변수 
    public AudioClip reloadSfx;

    //적 캐릭터의 총알 프리팹 
    public GameObject Bullet;
    //총알의 발사 위치 정보 
    public Transform firePos;


    void Start()
    {
        //컴포넌트 추출  / 변수 저장 
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);

    }

    void Update()
    {
        if(!isReload && isFire)
        {
            //현재 시간이 다음 발사 시간보다 큰지 여부를 확인 
            if(Time.time >= nextFire)
            {
                Fire();
                //다음 발사 시간 계산 
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            //주인공이 있는 위치까지의 회전 각도 계산
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            //보간 함수를 사용해 점진적으로 회전시킴 
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 1.0f);
        //총알 생성 
        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        Destroy(_bullet, 3.0f);

        // 남은 총알로 재장전 여부를 게산
        isReload = (--currBullet % maxBullet == 0);
        if(isReload)
        {
            StartCoroutine(Reloading());
        }

    }
    
    IEnumerator Reloading()
    {
        //재장전 애니메이션 실행
        animator.SetTrigger(hashReload);
        //재장전 사운드 
        audio.PlayOneShot(reloadSfx, 1.0f);
        //재장전 시간만크 대기하는 동안 제어권을 양보
        yield return wsReload;
        //총알 개수 초기화 
        currBullet = maxBullet;
        isReload = false;
    }



}
