using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
}

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
    public bool isRoll = false;                                 //회피
    private bool isReloading = false;                           //재장전
    public Text currentWeapon;                                  //현재 무기 종류 UI Text
    public Text ammoInfo;                                       //장탄수 정보 UI Text
    public ParticleSystem muzzleFlash;                          //총구 화염효과 Effect;
    private AudioSource _audio;                                 //Audio Component
    public PlayerSfx playerSfx;

    [Header("ASSULT RIFLE")]
    public float bulletSpeed = 3000.0f;                         //Bullet Speed
    public float bulletDamage = 10.0f;                          //Bullet Damage
    public int maxRifleAmmo = 30;                               //최대 돌격소총의 장탄
    public int currRifleAmmo;                                   //현재 돌격소총의 장탄
    private float rayDistance = 100.0f;                         //RayCast 발사 거리
    private float rapidFire = 0.0f;
    private float fireDelay = 0.1f;                              //사격 딜레이

    [Header("SHOT GUN")]
    public int pelletCount = 5;                                 //발사될 총알 갯수
    public float spreadAngle = 10.0f;                           //총알의 퍼짐 각도
    public float shotGunBulletSpeed = 1000.0f;                  //산탄총 탄속
    public int maxShotAmmo = 8;                                  //최대 산탄총 장탄
    public int currShotAmmo;                                    //현재 산탄총 장탄
    List<Quaternion> pellets;                                   //랜덤한 각도를 담을 List

    [Header("SNIPER RIFLE")]
    public float sniperRifleDamage = 80.0f;                     //스나이퍼 라이플 대미지
    public int maxSniperAmmo = 3;                               //최대 저격총 장탄
    public int currSniperAmmo;                                  //현재 저격총 장탄
    //public bool isSniperCritical = false;                     //저격총의 치명타 여부
    //private int sniperCriticalChace = 4;                      //저격총의 치명타 확률
    private LineRenderer laserPointer;

    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");


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

        _audio = GetComponent<AudioSource>();

        //스나이퍼 라이플의 레이저포인터 설정.
        laserPointer = GetComponent<LineRenderer>();
        laserPointer.SetColors(Color.red, Color.yellow);
        laserPointer.SetWidth(0.025f, 0.025f);
        laserPointer.enabled = false;

        anim = GetComponent<Animator>();

        //장탄수 설정.
        currRifleAmmo = maxRifleAmmo;
        currShotAmmo = maxShotAmmo;
        currSniperAmmo = maxSniperAmmo;

        UpdateWeaponInfo();
        UpdateAmmoInfo();
    }

    private void Update()
    {
        //조준모드
        if (isAiming && !isRoll)
        {
            if(weaponType == WEAPONTYPE.ASSULT_RIFLE)
            {
                laserPointer.enabled = false;

                if(Input.GetMouseButton(0) && !isReloading)
                {
                    rapidFire += Time.deltaTime;
                
                    if(rapidFire >= fireDelay)
                    {
                        --currRifleAmmo;
                        BulletFire();
                        rapidFire = 0.0f;
                        muzzleFlash.startRotation = firePos.position.y;
                        muzzleFlash.Play();
                        FireSfx(1);
                    }
                }

                if(currRifleAmmo == 0)
                {
                    StartCoroutine(this.Reload());

                    //anim.SetTrigger(hashReload);
                }
            }

            else if (weaponType == WEAPONTYPE.SHOT_GUN)
            {
                laserPointer.enabled = false;

                if(Input.GetMouseButtonDown(0) && !isReloading)
                {
                    ShotGunFire();
                    anim.SetTrigger(hashFire);
                    --currShotAmmo;
                    UpdateAmmoInfo();
                    FireSfx(2);
                }

                if(currShotAmmo == 0)
                {
                    StartCoroutine(this.Reload());

                    //anim.SetTrigger(hashReload);
                }
            }

            if(weaponType == WEAPONTYPE.SNIPER_RIFLE)
            {
                RaycastHit[] hits;

                hits = Physics.RaycastAll(firePos.position, firePos.forward, rayDistance);

                //RayHit와 정확도 부정확할 수 있습니다.
                //완벽하게 정확도 맞추려면 for문 아래에 lasetPointer 주석풀고 그거쓰면 됩니다.
                //laserPointer.enabled = true;
                //laserPointer.SetPosition(0, firePos.position);
                //laserPointer.SetPosition(1, firePos.forward * rayDistance);

                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];

                    Debug.DrawRay(firePos.position, firePos.forward);

                    laserPointer.enabled = true;
                    laserPointer.SetPosition(0, firePos.position);
                    laserPointer.SetPosition(1, hit.point + firePos.forward * rayDistance);
                    
                    if (Input.GetMouseButtonDown(0) && !isReloading)
                    {
                        if (hit.collider.tag == "ENEMY")
                        {
                            //int randomCritical = Random.Range(1, 11);

                            //스나이퍼 라이플 치명타 확률 랜덤.
                            //스나이퍼 라이플 치명타 방법 생각해보기.
                            //if(randomCritical <= sniperCriticalChace)
                            //{
                            //    isSniperCritical = true;
                            //}
                            //else
                            //{
                            //    isSniperCritical = false;
                            //}

                            object[] _infos = new object[2];
                            _infos[0] = hit.point;              //Ray에 맞은 위치값.
                            _infos[1] = sniperRifleDamage;      //Enemy 에 전달할 대미지 값.
                            //_infos[2] = isSniperCritical;
                    
                            hit.collider.gameObject.SendMessage("OnDamage",
                                _infos,
                                SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }

                if(Input.GetMouseButtonDown(0) && !isReloading)
                {
                    //sniperRifleDamage += Random.Range(sniperMinDamage, sniperMaxDamage);
                    --currSniperAmmo;
                    UpdateAmmoInfo();
                    FireSfx(3);
                }

                if(currSniperAmmo == 0)
                {
                    StartCoroutine(this.Reload());

                    //anim.SetTrigger(hashReload);
                }
            }
        }
        if(!isAiming)
        {
            laserPointer.enabled = false;
        }

        ChangeWeapon();
    }

    void BulletFire()
    {
        RaycastHit _hit;
        
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(_ray, out _hit);
        
        Vector3 bulletPoint = new Vector3(_hit.point.x, firePos.position.y, _hit.point.z);
        
        GameObject bullet = Instantiate(bulletPrefabs, firePos.position, firePos.rotation) as GameObject;
        bullet.transform.LookAt(bulletPoint);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
        anim.SetTrigger(hashFire);

        UpdateAmmoInfo();
    }

    void ShotGunFire()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            pellets[i] = Random.rotation;
        
            GameObject shot = Instantiate(shotGunPrefabs, firePos.position, firePos.rotation) as GameObject;
            shot.transform.rotation = Quaternion.RotateTowards(shot.transform.rotation, pellets[i], spreadAngle);
            shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * shotGunBulletSpeed);
        }

        anim.SetTrigger(hashFire);

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

    IEnumerator Reload()
    {
        anim.SetTrigger(hashReload);
        UpdateAmmoInfo();

        isReloading = true;

        currRifleAmmo = maxRifleAmmo;
        currShotAmmo = maxShotAmmo;
        currSniperAmmo = maxSniperAmmo;

        yield return new WaitForSeconds(1.2f);



        UpdateAmmoInfo();

        isReloading = false;
    }

    void UpdateWeaponInfo()
    {
        if (weaponType == WEAPONTYPE.ASSULT_RIFLE)
        {
            currentWeapon.text = "ASSULT RIFLE";
            currentWeapon.color = Color.yellow;
        }

        else if (weaponType == WEAPONTYPE.SHOT_GUN)
        {
            currentWeapon.text = "SHOT GUN";
            currentWeapon.color = Color.red;
        }

        else if (weaponType == WEAPONTYPE.SNIPER_RIFLE)
        {
            currentWeapon.text = "SNIPER RIFLE";
            currentWeapon.color = Color.green;
        }
    }

    void ChangeWeapon()
    {
        if (isReloading) return;

        //무기교체
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponType = WEAPONTYPE.ASSULT_RIFLE;
            UpdateWeaponInfo();
            UpdateAmmoInfo();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponType = WEAPONTYPE.SHOT_GUN;
            UpdateWeaponInfo();
            UpdateAmmoInfo();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponType = WEAPONTYPE.SNIPER_RIFLE;
            UpdateWeaponInfo();
            UpdateAmmoInfo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(this.Reload());
        }
    }

    void UpdateAmmoInfo()
    {
        if(weaponType == WEAPONTYPE.ASSULT_RIFLE)
        {
            ammoInfo.text = string.Format("<color=#ff0000>{0} </color>/ {1}", currRifleAmmo, maxRifleAmmo);
        }

        else if (weaponType == WEAPONTYPE.SHOT_GUN)
        {
            ammoInfo.text = string.Format("<color=#ff0000>{0} </color>/ {1}", currShotAmmo, maxShotAmmo);
        }

        else if (weaponType == WEAPONTYPE.SNIPER_RIFLE)
        {
            ammoInfo.text = string.Format("<color=#ff0000>{0} </color>/ {1}", currSniperAmmo, maxSniperAmmo);
        }
    }

    void FireSfx(int soundNum)
    {
        var _sfx = playerSfx.fire[soundNum];
        //사운드 발생
        _audio.PlayOneShot(_sfx, 1.0f);
    }

}
