using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    private float rotSpeed = 80.0f;
    private float r = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        r = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);
    }
}
