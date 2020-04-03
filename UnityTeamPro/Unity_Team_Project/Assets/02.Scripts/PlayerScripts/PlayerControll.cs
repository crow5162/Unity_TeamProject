﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Header("Character Speed")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float rotSpeed = 80.0f;

    private float r = 0.0f;

    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;

    float turnSmoothVelocity;
    float speedSmoothVelocity; 

    float currentSpeed;

    Transform cameraTr;

    private Animator anim;
    private Transform playerTr;
    private CameraControll cControll;

    private readonly int hashMoving = Animator.StringToHash("isMove");
    private readonly int hashAiming = Animator.StringToHash("isAiming");
    private readonly int hashV = Animator.StringToHash("v");
    private readonly int hashH = Animator.StringToHash("h");

    [Header("Aiming Target")]
    public float _targetRadius = 3.0f; //타겟검출 범위 지정
    public Collider[] targets;
    private bool isAiming = false;      //조준모드
    private bool isLockTarget = false;  //타겟을 조준
    private GameObject target;
    private Quaternion targetRotation;
    private FireControll fireControll;  

    // Start is called before the first frame update
    void Start()
    {
        cameraTr = Camera.main.transform;
        anim = GetComponent<Animator>();
        playerTr = GetComponent<Transform>();

        target = GameObject.FindWithTag("ENEMY");

        fireControll = GetComponent<FireControll>();
    }

    Transform FindTargets()
    {
        float maxDist = _targetRadius;
        Transform nearest = null;
        targets = Physics.OverlapSphere(transform.position, _targetRadius);

        foreach (Collider hit in targets)
        {
            if (hit && hit.tag == "ENEMY")
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);

                if (dist < maxDist)
                {
                    maxDist = dist;
                    nearest = hit.transform;
                }
            }
        }

        return nearest;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        //CharacterRotation
        if (inputDir != Vector2.zero)
        {
            if (!isLockTarget)
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTr.eulerAngles.y;
                transform.eulerAngles = Vector2.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            }
        }

        Vector3 moveDir = (Vector3.forward * input.y) + (Vector3.right * input.x);
        
        float speed = ((isAiming) ? walkSpeed : runSpeed) * inputDir.magnitude;
        
        currentSpeed = Mathf.SmoothDamp(currentSpeed, speed, ref speedSmoothVelocity, speedSmoothTime);
        
        //이동처리
        //Aiming 상태가 아닐때에만 이동처리를 합니다.
        if(!isLockTarget)
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        if (isLockTarget)
        transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);


        //Aiming Animation OutPut
        anim.SetFloat(hashH, input.x);
        anim.SetFloat(hashV, input.y);

        if (input.x == 0 && input.y == 0)
        {
            anim.SetBool(hashMoving, false);
        }
        else if (input.x != 0 || input.y != 0)
        {
            anim.SetBool(hashMoving, true);
        }
        
        if(Input.GetMouseButton(1))
        {
            isAiming = true;
            fireControll.isAiming = true;

        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            isLockTarget = false;
            fireControll.isAiming = false;
        }

        Aiming();
    }

    private void LateUpdate()
    {
        
    }

    void Aiming()
    {
        if(isAiming)
        {

            Transform target = FindTargets();

            if(target != null)
            {
                Vector3 autoAim = new Vector3(target.transform.position.x,
                                              transform.position.y,
                                              target.transform.position.z);
                
                playerTr.transform.LookAt(autoAim);
                
                isLockTarget = true;
                
            }

            else if (target == null)
            {
                isLockTarget = false;
            }   
        }

        anim.SetBool(hashAiming, isAiming);
    }

    void FootStep()
    {

    }
}
