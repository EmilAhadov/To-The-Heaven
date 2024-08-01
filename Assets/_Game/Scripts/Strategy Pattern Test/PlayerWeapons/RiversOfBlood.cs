using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiversOfBlood : Swords , IAttackable
{
    public RiversOfBlood()
    {
        _baseDamage = 74;
    }

    public void Attack(int damage)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        Debug.Log($"Sword damage: {_baseDamage}");
        Debug.Log($"Sword attack count: {_attackCountPerTurn}");
    }
}
