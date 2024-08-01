using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    protected int _baseDamage;
    public int _currentDamage;
    protected int _attackCountPerTurn;

    //protected List<IBonusDamage> _bonusDamages;
    protected IAttackable _attackable;

    public void Attack()
    {
        _attackable?.Attack(_currentDamage);
    }

    public void AddBonusType(IBonusDamage bonusType)
    {
        //_bonusDamages.Add(bonusType);
    }
}

