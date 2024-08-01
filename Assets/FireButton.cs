using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireButton : MonoBehaviour
{
    void Update()
    {
        if(PlayerController.Instance.CanShot)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
