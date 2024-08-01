using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackKnightHalberd : Axes , IAttackable
{
    private void Start()
    {
        _baseDamage = 133;
    }

    private void Update()
    {
        //Debug.Log($"Halberd damage: {_baseDamage}");
        //Debug.Log($"Halberd attack count: {_attackCountPerTurn}");
    }

    public void Attack(int damage)
    {
        throw new System.NotImplementedException();
    }
}
