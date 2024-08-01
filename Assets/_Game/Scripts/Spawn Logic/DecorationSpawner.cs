using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSpawner : MonoBehaviour
{
    //Game Objects
    private GameObject obj;
    [SerializeField] private ObjectPool objectPool = null;

    private int previosIndex = 100;
    
    public void SpawnDecor(Vector2 targerPosition)
    {
        int index = Random.Range(0, objectPool.Pools.Length);
        if (index == previosIndex)
        {
            if (index == 0)
            {
                index++;
            }
            else
            {
                index--;
            }
        }
        previosIndex = index;
        obj = objectPool.GetPooledObject(index);
        obj.transform.position = targerPosition;
    }
}
