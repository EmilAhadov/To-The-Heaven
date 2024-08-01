using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval;

    //Game Objects
    private GameObject obj;

    [SerializeField] private ObjectPool objectPool = null;
    [SerializeField] private Transform _secondFloorSpawnerTransform;

    [SerializeField] private float _goldDistance;
     public float GoldDistance { get {return _goldDistance;} private set { } }

    //Second Floor Fields
    private bool _isSpawningForSecondFloor;
    private bool _spawnGoldForSecondFloor;


    public bool firstOnceWingPerTurn;
    public bool secondOnceWingTurn;


    void Start()
    {
        _isSpawningForSecondFloor = true;
        _spawnGoldForSecondFloor = false;
    }

    private void Update()
    {
        if(_spawnGoldForSecondFloor)
        {
            SecondFloorStateHandler();
        }
    }

    private void SecondFloorStateHandler() 
    {
        if (_isSpawningForSecondFloor)
        {
            StartCoroutine(SpawnGoldForSecondFloor());
        }
    }

    public void SpawnGold(int goldCount)
    {
        for(int i = 0; i < goldCount; i++)
        {
            obj = objectPool.GetPooledObject(0);
            obj.transform.position = transform.position;
            transform.position = new Vector2(transform.position.x + _goldDistance, transform.position.y);
        }
    }

    private IEnumerator SpawnGoldForSecondFloor() 
    {
        _isSpawningForSecondFloor = false;
        obj = objectPool.GetPooledObject(0);
        obj.transform.position = _secondFloorSpawnerTransform.position;
        //_secondFloorSpawnerTransform.position = new Vector2(_secondFloorSpawnerTransform.position.x + _horizontalOffset, _secondFloorSpawnerTransform.position.y);
        yield return new WaitForSeconds(_spawnInterval);
        _isSpawningForSecondFloor = true;
    }

    //public void SpawnSpecialCollectable(int poolIndex)
    //{
    //    obj = objectPool.GetPooledObject(poolIndex);
    //    obj.transform.position = transform.position;
    //    transform.position = new Vector2(transform.position.x + _goldDistance, transform.position.y);
    //}

    public void SpawnRandomSpecialCollectable()
    {
        if (HealthSystem.Instance.hitPoint >= 60)
        {
            obj = objectPool.GetPooledObject(Random.Range(1, 4));
        }
        else if (HealthSystem.Instance.hitPoint >= 40)
        {
            obj = objectPool.GetPooledObject(Random.Range(1, 5));
        }
        else if (HealthSystem.Instance.hitPoint >= 20)
        {
            obj = objectPool.GetPooledObject(Random.Range(1, 6));
        }
        else
        {
            obj = objectPool.GetPooledObject(Random.Range(1, 7));
        }
        obj.transform.position = transform.position;
        transform.position = new Vector2(transform.position.x + _goldDistance, transform.position.y);
    }



    public void SpawnRandomSpecialCollectableForBuilding()
    {
        float gambleVariable = Random.Range(0, 18);
        if (LevelManager.Instance._levelState == LevelManager.levelState.levelState4 && gambleVariable > 9 && firstOnceWingPerTurn && !PlayerEventPerformer.Instance._isFirstFinishItemTaken)
        {
            obj = objectPool.GetPooledObject(8);
            firstOnceWingPerTurn = false;
        }
        else if (LevelManager.Instance._levelState == LevelManager.levelState.levelState6 && gambleVariable > 9 && secondOnceWingTurn && !PlayerEventPerformer.Instance._isSecondFinishItemTaken)
        {
            obj = objectPool.GetPooledObject(9);
            secondOnceWingTurn = false;
        }
        else
        {
            if (HealthSystem.Instance.hitPoint >= 60)
            {
                obj = objectPool.GetPooledObject(Random.Range(1, 4));
            }
            else if (HealthSystem.Instance.hitPoint >= 40)
            {
                obj = objectPool.GetPooledObject(Random.Range(1, 5));
            }
            else if (HealthSystem.Instance.hitPoint >= 20)
            {
                obj = objectPool.GetPooledObject(Random.Range(1, 6));
            }
            else
            {
                obj = objectPool.GetPooledObject(Random.Range(1, 7));
            }
        }
        obj.transform.position = transform.position;
        transform.position = new Vector2(transform.position.x + _goldDistance, transform.position.y);
    }


    public void SetPassiveAllObjects()
    {
        objectPool.SetPassiveAllObjects();
    }
}
