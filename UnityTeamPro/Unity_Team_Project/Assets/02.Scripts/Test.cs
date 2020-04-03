using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Delegate();
public delegate void Delegate<T>(T t);
public delegate void Delegate<T1, T2>(T1 t1, T2 t2);

public delegate T DelegateR<T>();

public class Test : MonoBehaviour
{
    public event Delegate<Collider> EventTriggerEnter;
    public event Delegate<Collider> EventTriggerExit;

    public void OnTriggerEnter(Collider other)
    {
        EventTriggerEnter?.Invoke(other);
    }


    public void OnTriggerExit(Collider other)
    {
        if (EventTriggerExit != null)
            EventTriggerExit(other);
    }
}
