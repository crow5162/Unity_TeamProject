using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControll : MonoBehaviour
{
    public enum BULLETTYPE
    {
        ASSULTRIFLE_BULLET,
        SHOTGUN_BULELT,
    }

    public BULLETTYPE bulletType;

    private float destroyTime = 2.0f;

    public float bulletDamage = 10.0f;

    private int minDamageRange = -3;
    private int maxDamageRange = 3;

    private int criticalChance = 3;
    private float criticalDamageAssult = 3.5f;
    private float criticalDamageShot = 1.3f;
    public bool isCritical = false;

    public GameObject bulletEffect;

    private const string enemyTag = "ENEMY";
    private const string wallTag = "WALL";

    private void Awake()
    {
        bulletDamage += Random.Range(minDamageRange, maxDamageRange);

        int randomDamage = Random.Range(1, 11);
        if(randomDamage <= criticalChance)
        {
            if(bulletType == BULLETTYPE.ASSULTRIFLE_BULLET)
            {
                TrailRenderer trail = this.gameObject.GetComponent<TrailRenderer>();
                trail.startColor = Color.red;
                trail.endColor = Color.red;
                trail.startWidth = 0.3f;
                trail.endWidth = 0.1f;

                bulletDamage *= criticalDamageAssult;
                //형변환 안해주면 소숫점 나옵니다.
                bulletDamage = (int)bulletDamage;
                isCritical = true;
            }

            else if (bulletType == BULLETTYPE.SHOTGUN_BULELT)
            {
                bulletDamage *= criticalDamageShot;
                //형변환 안해주면 소숫점 나옵니다.
                bulletDamage = (int)bulletDamage;
                isCritical = true;
            }
        }
        else
        {
            isCritical = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == enemyTag)
        {
            //Assult Rifle Effect 호출.
            AssultRifleEffect(coll);
        }

        else if (coll.gameObject.tag == wallTag)
        {
            Destroy(this.gameObject);
        }
    }

    void AssultRifleEffect(Collision coll)
    {
        //충돌지점 정보 획득.
        ContactPoint contact = coll.contacts[0];

        GameObject riffleffect = Instantiate(bulletEffect, contact.point, Quaternion.identity) as GameObject;
        float particleTime = riffleffect.GetComponent<ParticleSystem>().duration;

        Destroy(riffleffect, particleTime);
    }
}
