using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;
    private Transform tr;

    public float MoveSpeed = 10.0f;
    public float rotSpeed = 60.0f;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        float MouseX = Input.GetAxis("Mouse X");


        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * MoveSpeed * Time.deltaTime, Space.Self);

        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * MouseX);
        //tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * h);
    }
}
