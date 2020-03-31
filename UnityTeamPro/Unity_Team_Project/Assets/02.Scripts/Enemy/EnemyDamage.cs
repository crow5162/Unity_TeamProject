using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    //생명 게이지 
    private float hp = 150.0f;

    void Start()
    {

        
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == bulletTag)
        {
            //총알 삭제 
            Destroy(coll.gameObject);
            //생명게이지 차감 
            hp -= coll.gameObject.GetComponent<EnemyBullet>().damage;
            if(hp<=0.0f)
            {
                //적 캐릭터의 상태를 DIE로 변경
                GetComponent<Enemy>().EnemyState = Enemy.CurrentState.Die;
            }
        }
    }

    void Update()
    {
        
    }
}
