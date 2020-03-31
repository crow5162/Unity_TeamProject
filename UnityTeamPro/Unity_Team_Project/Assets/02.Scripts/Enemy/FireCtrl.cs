using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    //총알 발사 프리팹
    public GameObject bullet;
    //총알 발사 좌표 
    public Transform firePos;

    void Start()
    {
        
    }

    void Update()
    {
      //Fire();
    }

    void Fire()
    {
        //bullet 프리팹을 동적으로 생성
        Instantiate(bullet, firePos.position, firePos.rotation);
    }
}
