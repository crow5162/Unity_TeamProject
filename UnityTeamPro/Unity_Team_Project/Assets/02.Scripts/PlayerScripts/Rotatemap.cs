using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatemap : MonoBehaviour
{
    private Transform mapTransform;

    private void Awake()
    {
        mapTransform = GameObject.Find("MapObject").GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "PLAYER")
        //{
        //    //transform.rotation = Quaternion.Euler()
        //
        //    Debug.Log("Collision On Box !!");
        //}

        Debug.Log("Collision On Box !!");

        mapTransform.eulerAngles = new Vector3(50, 50, 50);
    }
}
