using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{

    //총알 뎀지 
    public float damage = 20.0f;
    //총알 발사 속도 
    public float speed = 500.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
    }

    void Update()
    {

    }
}
