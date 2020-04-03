using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    //public float rotateSpeed = 10.0f;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("PLAYER").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        //}
        //
        //else if (Input.GetKey(KeyCode.E))
        //{
        //
        //}

        this.transform.RotateAround(playerTransform.position, Vector3.up, 90.0f * Time.deltaTime);
    }
}
