using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private GameObject obj;

    [SerializeField] private ObjectPool objectPool = null;

    public void SpawnObstacle(Vector2 targetPosition)
    {
        int index = Random.Range(0, objectPool.Pools.Length);
        obj = objectPool.GetPooledObject(index);
        obj.transform.position = targetPosition;
    }

    public void SetPassiveAllObjects()
    {
        objectPool.SetPassiveAllObjects();
    }

}
