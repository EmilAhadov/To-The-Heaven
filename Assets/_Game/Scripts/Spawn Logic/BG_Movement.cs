using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _posX;
    void Update()
    {
        _posX = transform.position.x;
        _posX -= _speed * Time.deltaTime;
        transform.position = new Vector2(_posX,transform.position.y);
    }
}
