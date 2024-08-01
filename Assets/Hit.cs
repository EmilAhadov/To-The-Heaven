using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hit : MonoBehaviour
{
    [SerializeField] private GameObject _fireball;
    public void DestroyFireball()
    {
        _fireball.SetActive(false);
    }
}
