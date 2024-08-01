using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldStrategy", menuName = "ScriptableObjects/Spells/FireballStrategy")]
public class FireBallAbility : AbilityStrategy
{
    public override void UseAbility(Transform origin)
    {
        //Instantiate(_fireballPrefab, new Vector2(origin.position.x + 3, origin.position.y), Quaternion.identity);
        //_fireballPrefab.SetActive(true);
        PlayerController.Instance.CanShot = true;
    }
}
