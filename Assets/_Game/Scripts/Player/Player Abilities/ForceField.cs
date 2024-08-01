using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShieldStrategy", menuName = "ScriptableObjects/Spells/ShieldStrategy")]
public class ForceField : AbilityStrategy
{
    [SerializeField] private GameObject _shieldPrefab;
    public override void UseAbility(Transform origin)
    {
        //Instantiate(_shieldPrefab, origin, )
        Debug.Log("Shield Activated");
        EventHolder.Instance.OnActivateShield();
        //_shieldPrefab
    
    }
}
