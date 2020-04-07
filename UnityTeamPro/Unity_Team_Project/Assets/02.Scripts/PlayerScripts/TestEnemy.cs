using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private float testHp = 50.0f;
    private float currentHp;

    private MeshRenderer meshRenderer;
    private CapsuleCollider capCollider;

    public GameObject sniperRifleEffect;

    public GameObject hudDamageText;
    public Transform hudPos;

    private int sniperMinDamage = -3;
    private int sniperMaxDamage = 3;

    //private AudioSource _audio;
    //public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        capCollider = GetComponent<CapsuleCollider>();
        //_audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        currentHp = testHp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "BULLET")
        {
            float bulletDamage = coll.gameObject.GetComponent<BulletControll>().bulletDamage;

            if(coll.gameObject.GetComponent<BulletControll>().isCritical)
            {
                TakeDamage(bulletDamage, Color.yellow);
            }
            else
            {
                TakeDamage(bulletDamage, Color.white);
            }

            currentHp -= bulletDamage;

            Destroy(coll.gameObject);

            if (currentHp <= 0)
            {
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    //Player SniperRifle Hitting
    void OnDamage(object[] _infos)
    {
        //if((bool)_infos[2])
        //{ 
        //    float sniperRifleDamage = (float)_infos[1];
        //    sniperRifleDamage *= 1.3f;
        //    sniperRifleDamage = (int)sniperRifleDamage;
        //    currentHp -= sniperRifleDamage;
        //}
        //else
        //{
        //    currentHp -= (float)_infos[1];
        //}

        float sniperDamage = (float)_infos[1];
        sniperDamage = (int)sniperDamage;
        sniperDamage += Random.Range(sniperMinDamage, sniperMaxDamage);

        //currentHp -= (float)_infos[1];

        Debug.Log(sniperDamage.ToString());

        currentHp -= sniperDamage;

        TakeDamage(sniperDamage, Color.red);

        CreateSniperRifleEffect((Vector3)_infos[0]);

        if (currentHp <= 0)
        {
            StartCoroutine(SpawnEnemy());

        }
    }

    IEnumerator SpawnEnemy()
    {
        capCollider.enabled = false;
        meshRenderer.enabled = false;
        //_audio.clip = clips[1];
        //_audio.Play();

        yield return  new WaitForSeconds(3.0f);

        capCollider.enabled = true;
        meshRenderer.enabled = true;
        currentHp = testHp;
    }

    void CreateSniperRifleEffect(Vector3 pos)
    {
        GameObject effect = (GameObject)Instantiate(sniperRifleEffect, pos, Quaternion.identity) as GameObject;
        float particleTime = effect.GetComponent<ParticleSystem>().duration;
        Destroy(effect, particleTime);
    }

    public void TakeDamage(float Damage, Color color)
    {
        GameObject hudText = Instantiate(hudDamageText) as GameObject;         //생성할 텍스트 오브젝트
        hudText.transform.position = hudPos.position;                          //Damage 수치가 표시될 위치.
        hudText.GetComponent<DamageCount>().damage = Damage;
        hudText.GetComponent<DamageCount>().criticalColor = color;
    }
}
