using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDetector : MonoBehaviour
{
    public static bool DetectMobile()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
