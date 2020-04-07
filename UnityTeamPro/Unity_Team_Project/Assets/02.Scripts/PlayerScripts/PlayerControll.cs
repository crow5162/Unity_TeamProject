using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField]

    Transform cameraPos;
    [Header("Character Speed")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float rotSpeed = 80.0f;

    private float rollSpeed = 200.0f;

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
    private Rigidbody rb;
    private bool isRoll = false;

    [HideInInspector]
    public bool isDead = false;
    public Collider[] targets;

    private readonly int hashMoving = Animator.StringToHash("isMove");
    private readonly int hashAiming = Animator.StringToHash("isAiming");
    private readonly int hashRoll = Animator.StringToHash("Roll");
    private readonly int hashV = Animator.StringToHash("v");
    private readonly int hashH = Animator.StringToHash("h");
    private readonly int hashDeath = Animator.StringToHash("isDead");

    [Header("Aiming Target")]
    public float _targetRadius = 3.0f; //타겟검출 범위 지정
    private bool isAiming = false;      //조준모드
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
        rb = GetComponent<Rigidbody>();
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
        if(isDead) return;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        //CharacterRotation
        if (inputDir != Vector2.zero)
        {
            if (!isAiming && !isRoll)
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
        if(!isAiming && !isRoll)
        {
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
            cameraPos.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        }

        if (input.x == 0 && input.y == 0)
        {
            anim.SetBool(hashMoving, false);
        }
        else if (input.x != 0 || input.y != 0)
        {
            anim.SetBool(hashMoving, true);
        }

        //Aiming Animation OutPut
        anim.SetFloat(hashH, input.x);
        anim.SetFloat(hashV, input.y);

        
        if(Input.GetMouseButton(1))
        {
            isAiming = true;
            fireControll.isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            fireControll.isAiming = false;
        }

        Aiming();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isRoll)
            {
                StartCoroutine(this.RollAction());

            }

        }
    }

    private void FixedUpdate()
    {
        if(isRoll)
        {
            rb.AddForce(transform.forward * rollSpeed);
        }
    }

    void Aiming()
    {
        if(isAiming && !isRoll)
        {
            //Transform target = FindTargets();
            //
            //if(target != null)
            //{
            //    Vector3 autoAim = new Vector3(target.transform.position.x,
            //                                  transform.position.y,
            //                                  target.transform.position.z);
            //
            //
            //    //float targetRotation = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg + target.eulerAngles.y;
            //    //transform.eulerAngles = Vector2.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            //
            //    playerTr.transform.LookAt(autoAim);
            //
            //    isLockTarget = true;
            //    
            //}
            //
            //else if (target == null)
            //{
            //    isLockTarget = false;
            //}   

            float _horizontal = Input.GetAxisRaw("Horizontal");
            float _vertical = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);

            Vector3 _movement = new Vector3(_horizontal, 0, _vertical);
            _movement = _movement.normalized;

            //transform.Translate(_movement * walkSpeed * Time.deltaTime, Space.Self);

            transform.Translate(_movement * walkSpeed * Time.deltaTime, Space.World);

            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;

            if(playerPlane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
            }
        }

        anim.SetBool(hashAiming, isAiming);
    }

    IEnumerator RollAction()
    {
        anim.SetTrigger(hashRoll);

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Vector3 nowforward = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            if (nowforward != Vector3.zero) transform.forward = nowforward;
            transform.Translate(Vector3.forward * Time.deltaTime);
        }

        isRoll = true;
        fireControll.isRoll = true;

        yield return new WaitForSeconds(0.85f);

        isRoll = false;
        fireControll.isRoll = false;
    }

#region AnimationEvent

    void FootStep()
    {
    
    }
    
    void RollSound()
    {
    
    }
    
    void CantRotate()
    {
    
    }
    
    void EndRoll()
    {
    
    }

#endregion

}
