//==============================================================
// Demo Buttons
//==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public void Button1()
    {
        HealthSystem.Instance.TakeDamage(10f); // Take damage 10 points
    }
    public void Button2()
    {
        HealthSystem.Instance.HealDamage(10f); // Heal damage 10 points
    }
}
