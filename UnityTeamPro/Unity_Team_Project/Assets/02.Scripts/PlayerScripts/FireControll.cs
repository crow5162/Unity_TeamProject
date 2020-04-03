using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControll : MonoBehaviour
{
    public enum WEAPONTYPE
    {
        NONE_WEAPON,
        ASSULT_RIFLE,
        SHOT_GUN,
        SNIPER_RIFLE,
    }

    [Header("Fire Controll")]
    public WEAPONTYPE weaponType = WEAPONTYPE.ASSULT_RIFLE;     //WEAPON TYPE
    public GameObject bulletPrefabs;                            //Rifle Bullet 프리팹 저장.
    public GameObject shotGunPrefabs;                           //Shot gun Prefabs.
    public Transform firePos;                                   //총알 발사 위치
    public bool isAiming = false;                               //조준모드 

    [Header("ASSULT RIFLE")]
    public float bulletSpeed = 3000.0f;                         //Bullet Speed
    public float bulletDamage = 10.0f;                          //Bullet Damage
    private float rayDistance = 100.0f;                         //RayCast 발사 거리
    private float rapidFire = 0.0f;
    private float fireDelay = 0.1f;                              //사격 딜레이

    [Header("SHOT GUN")]
    public int pelletCount = 5;                                 //발사될 총알 갯수
    public float spreadAngle = 10.0f;                           //총알의 퍼짐 각도
    public float shotGunBulletSpeed = 1000.0f;                  //산탄총 탄속    
    List<Quaternion> pellets;                                   //랜덤한 각도를 담을 List

    [Header("SNIPER RIFLE")]
    public float sniperRifleDamage = 50.0f;                     //스나이퍼 라이플 대미지
    private LineRenderer laserPointer;

    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("Fire");


    // Start is called before the first frame update
    void Awake()
    {
        pellets = new List<Quaternion>(pelletCount);

        for (int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }

        //처음 무기 타입 지정
        weaponType = WEAPONTYPE.ASSULT_RIFLE;

        //스나이퍼 라이플의 레이저포인터 설정.
        laserPointer = GetComponent<LineRenderer>();
        laserPointer.SetColors(Color.red, Color.yellow);
        laserPointer.SetWidth(0.025f, 0.025f);
        laserPointer.enabled = false;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //조준모드
        if (isAiming)
        {
            if(weaponType == WEAPONTYPE.ASSULT_RIFLE)
            {
                laserPointer.enabled = false;

                if(Input.GetMouseButton(0))
                {
                    rapidFire += Time.deltaTime;
                
                    if(rapidFire >= fireDelay)
                    {
                        BulletFire();
                        rapidFire = 0.0f;
                    }
                }


            }

            else if (weaponType == WEAPONTYPE.SHOT_GUN)
            {
                laserPointer.enabled = false;

                if(Input.GetMouseButtonDown(0))
                {
                    ShotGunFire();
                    anim.SetTrigger(hashFire);
                }
            }

            if(weaponType == WEAPONTYPE.SNIPER_RIFLE)
            {
                RaycastHit[] hits;

                //Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                hits = Physics.RaycastAll(firePos.position, firePos.forward, rayDistance);

                laserPointer.enabled = true;
                laserPointer.SetPosition(0, firePos.position);
                laserPointer.SetPosition(1, firePos.forward * rayDistance);

                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];

                    Debug.DrawRay(firePos.position, hit.point);

                    //laserPointer.enabled = true;
                    //laserPointer.SetPosition(0, firePos.position);
                    //laserPointer.SetPosition(1, hit.point + firePos.forward * rayDistance);
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (hit.collider.tag == "ENEMY")
                        {
                            object[] _infos = new object[2];
                            _infos[0] = hit.point;               //Ray에 맞은 위치값.
                            _infos[1] = sniperRifleDamage;       //Enemy 에 전달할 대미지 값.
                    
                            hit.collider.gameObject.SendMessage("OnDamage",
                                _infos,
                                SendMessageOptions.DontRequireReceiver);
                        }
                    }


                }
            }
        }
        if(!isAiming)
        {
            laserPointer.enabled = false;
        }

        //무기교체
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponType = WEAPONTYPE.ASSULT_RIFLE;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponType = WEAPONTYPE.SHOT_GUN;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponType = WEAPONTYPE.SNIPER_RIFLE;
        }
    }

    void BulletFire()
    {
        RaycastHit _hit;
        
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Vector3 bulletPoint = new Vector3(_hit.point.x, firePos.position.y, );

        Physics.Raycast(_ray, out _hit);
        
        Vector3 bulletPoint = new Vector3(_hit.point.x, firePos.position.y, _hit.point.z);
        
        GameObject bullet = Instantiate(bulletPrefabs, firePos.position, firePos.rotation) as GameObject;
        bullet.transform.LookAt(bulletPoint);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
        anim.SetTrigger(hashFire);
        

    }

    void ShotGunFire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < pelletCount; i++)
            {
                pellets[i] = Random.rotation;
        
                if(Input.GetMouseButtonDown(0))
                {
                    GameObject shot = Instantiate(shotGunPrefabs, firePos.position, firePos.rotation) as GameObject;
                    shot.transform.rotation = Quaternion.RotateTowards(shot.transform.rotation, pellets[i], spreadAngle);
                    shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * shotGunBulletSpeed);
                }
            }

            anim.SetTrigger(hashFire);
        }
    }

    void SniperRifle()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(firePos.position, firePos.forward, rayDistance);

        for(int i =0;i<hits.Length;i++)
        {
            RaycastHit hit = hits[i];

            if(hit.collider.tag == "ENEMY")
            {
                object[] _infos = new object[2];
                _infos[0] = hit.point;               //Ray에 맞은 위치값.
                _infos[1] = sniperRifleDamage;       //Enemy 에 전달할 대미지 값.

                hit.collider.gameObject.SendMessage("OnDamage",
                    _infos,
                    SendMessageOptions.DontRequireReceiver);

            }
        }
    }

}
