using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private float _healthValue;

    public float HealthValue { get { return _healthValue; } private set { } }
}
