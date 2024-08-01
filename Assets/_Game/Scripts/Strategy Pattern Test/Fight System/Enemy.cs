using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string Name;
    protected int _damage;
    protected int _health;
    protected int _armorPoint;
    private int damage;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log(_health);
    }
}
