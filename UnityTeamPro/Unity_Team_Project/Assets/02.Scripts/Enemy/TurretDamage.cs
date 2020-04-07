using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurretDamage : MonoBehaviour
{

    //private const string bulletTag = "E_BULLET";
    //private const string enemyTag = "ENEMY";
    ////생명 게이지 
    //private float hp = 200.0f;
    ////초기 생명 수치 
    //private float initHp = 200.0f;
    //private bool isDie = false;

    ////생명 프리팹 저장할 변수 
    //public GameObject hpBarPrefab;
    ////위치 보정 
    //public Vector3 hpBarOffset = new Vector3(0, 3f, 0);
    ////부모 Canvas 객체 
    //private Canvas uiCanvas;
    //// 생명 수치에 따른 fill Amount 속성을 변경할 Image;
    //private Image hpBarImage;
    //private Enemy enemy;

    void Start()
    {
        //SetHpBar();
    }

    //void SetHpBar()
    //{
    //    uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
    //    //UI Canvas 하위로 생명 게이지를 생성 
    //    GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
    //    //fillAmount 속성을 변경할 Image를 추출 
    //    hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
    //    //생명 게이지가 따라가야 할 대상과 오프셋 값 설정 
    //    var _hpBar = hpBar.GetComponent<EnemyHpBar>();
    //    _hpBar.targetTr = this.gameObject.transform;
    //    _hpBar.offset = hpBarOffset;
    //}

    //void OnCollisionEnter(Collision coll)
    //{
    //    if (coll.collider.tag == "BULLET")
    //    {
    //        //피격효과 생성
    //        //ShowHitEffect(coll);
    //        //총알 삭제 
    //        Destroy(coll.gameObject);
    //        //생명게이지 차감 
    //        hp -= 15.0f;
    //        //Debug.Log(hp);
    //        hpBarImage.fillAmount = hp / initHp;

    //        if (hp <= 0.0f)
    //        {               
    //            //적 캐릭터가 사망한 이후 생명 게이지를 투명 처러ㅣ 
    //            hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
    //        }
    //    }
    //}
    //void ShowHitEffect(Collision coll)
    //{
    //    //총알이 충돌한 지점을 알아야함.
    //    Vector3 pos = coll.contacts[0].point;
    //    Vector3 _normal = coll.contacts[0].normal;
    //    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

    //    //피격 이펙트 생성
    //    //GameObject Hit = Instantiate<GameObject>(HitEffect, pos, rot);
    //    //Destroy(Hit, 1.0f);
    //}

    
}
