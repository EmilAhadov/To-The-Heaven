using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;

public class Finishing : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.GetComponent<CinemachineVirtualCamera>().Follow = null;
            camera.GetComponent<CinemachineVirtualCamera>().LookAt = null;
        }
    }
}
