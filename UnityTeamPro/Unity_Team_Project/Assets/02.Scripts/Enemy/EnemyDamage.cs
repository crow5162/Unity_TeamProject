using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "E_BULLET";
    private const string enemyTag = "ENEMY";
    //생명 게이지 
    public float hp = 150.0f;
    public bool isDie = false;


    // 피격시 사용할 효고 ( 이팩트 )
    private GameObject HitEffect;
    void Start()
    {
        //피격 효고ㅏ 
        HitEffect = Resources.Load<GameObject>("energyBlast");
        
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "BULLET")
        {
            //피격효과 생성
            ShowHitEffect(coll);
            //총알 삭제 
            Destroy(coll.gameObject);
            //생명게이지 차감 
            hp -= 15.0f;
            Debug.Log(hp);
            if(hp <= 0.0f)
            {
                //적 캐릭터의 상태를 DIE로 변경
                GetComponent<Enemy>().EnemyState = Enemy.CurrentState.Die;
            }
        }
    }
    void ShowHitEffect(Collision coll)
    {
        //총알이 충돌한 지점을 알아야함.
        Vector3 pos = coll.contacts[0].point;
        Vector3 _normal = coll.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        //피격 이펙트 생성
        GameObject Hit = Instantiate<GameObject>(HitEffect, pos, rot);
        Destroy(Hit, 1.0f);
    }


    void Update()
    {
        
    }
}
