using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegatesAndEvents : MonoBehaviour
{
    public delegate void TestDelegate();
    public static TestDelegate testDelegate;
    public static TestDelegate testDelegate1;
    public static event TestDelegate testEvent1;
    public static event Action testEvent2;
    public bool setAct = true;

    private void Start()
    {
        testDelegate += Method1;
        testDelegate += Method2;
        testDelegate += Method3;
        testEvent1 += Method5;
        testEvent1 += Method6;
        testEvent1 += Method1;
        testDelegate -= testEvent1;
    }
    private void Update()
    {
        testDelegate();
        if (Input.GetKeyDown(KeyCode.Escape))
            AddMethod();
        if(Input.GetKeyUp(KeyCode.W))
        {
            RemoveMethod();
        }
        testEvent1();
    }
    
    public void AddMethod()
    {
        testDelegate += Method4;
    }
    public void RemoveMethod()
    {
        testDelegate1 += Method1;
        testDelegate1 += Method2;

        testDelegate -= testDelegate1;
    }
    public void Method1()
    {
        Debug.Log("First method works");
    }

    public void Method2()
    {
        Debug.Log("Second mehod works");
    }

    public void Method3()
    {
        Debug.Log("Third method works");
    }

    public void Method4()
    {
        Debug.Log("Fourth method works");
    }

    public void Method5()
    {
        Debug.Log("Fifth method works");
    }

    public void Method6()
    {
        Debug.Log("Sixth method works");
    }

}
