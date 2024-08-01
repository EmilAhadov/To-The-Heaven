using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ColumnMovement : MonoBehaviour
{
    //[SerializeField] private float _verticalSpeed;
    //[SerializeField] private float _horizontalSpeed;


    private bool _onceWork;

    private int _objectIndex;

    private Vector2 _firstTarget;
    private Vector2 _secondTarget;
    private float _verticalOffset;
    private float _horizontalOffset;
    private Vector2 _currentTarget;

    //[SerializeField] private GameObject _testObject;
    private float gambleVariable;
    private void OnEnable()
    {
        StartCoroutine(Wait(0f));

       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Detected");

            //player = collision.gameObject;
            //transform.SetParent(collision.transform);
            collision.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    void FixedUpdate()
    {
        if (LevelManager.Instance._levelState == LevelManager.levelState.levelState5)
        {
            if (Vector2.Distance(_currentTarget, transform.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, _currentTarget, 2 * Time.deltaTime);
            }
            else
            {
                if (Vector2.Distance(_currentTarget, _firstTarget) > Vector2.Distance(_currentTarget, _secondTarget))
                {
                    _currentTarget = _firstTarget;
                }
                else
                {
                    _currentTarget = _secondTarget;
                }
            }
        }
        else if (LevelManager.Instance._levelState == LevelManager.levelState.levelState6)
        {
            if (Vector2.Distance(_currentTarget, transform.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, _currentTarget, 2 * Time.deltaTime);
            }
            else
            {
                if (Vector2.Distance(_currentTarget, _firstTarget) > Vector2.Distance(_currentTarget, _secondTarget))
                {
                    _currentTarget = _firstTarget;
                }
                else
                {
                    _currentTarget = _secondTarget;
                }
            }
        }
    }

    public void SetVariables()
    {
        SetForState5();



        if(LevelManager.Instance._levelState == LevelManager.levelState.levelState6 )
        {
            SetForState6();
        }
    }

    public void SetForState5()
    {
        gambleVariable = Random.Range(0, 2);

        _firstTarget = new Vector2(transform.position.x, -6);
        _secondTarget = new Vector2(transform.position.x, -10);
        _currentTarget = transform.position;
    }

    public void SetForState6()
    {
        _verticalOffset = Random.Range(3, 6);
        _horizontalOffset = Random.Range(1, 2);
        Vector2 obj = GroundSpawner.Instance.LastSpawnedObject;

        float distance = Vector2.Distance(transform.position, obj);
        Debug.Log($"Column distance is {distance}" +
            $"{obj}" +
            $"{transform.position}");
        if (distance >= 11.5f)
        {
            _firstTarget = new Vector2(transform.position.x, -11);
            _secondTarget = new Vector2(transform.position.x - _horizontalOffset, -11 + _verticalOffset);
        }
        else
        {
            _firstTarget = new Vector2(transform.position.x, -11);
            _secondTarget = new Vector2(transform.position.x + _horizontalOffset, -11 + _verticalOffset);
        }

        _currentTarget = transform.position;
    }

    public IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);

        _objectIndex = GroundSpawner.Instance.ObjectPool.Pools[1].pooledObjects.IndexOf(gameObject);
        
        SetVariables();
    }


}
