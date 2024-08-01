using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject _forceField;

    private void ActivateForceField()
    {
        _forceField.SetActive(true);
    }
}
