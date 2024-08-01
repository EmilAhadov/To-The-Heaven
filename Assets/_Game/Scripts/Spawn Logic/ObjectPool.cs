using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public List<GameObject> pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }

    [SerializeField] private Pool[] pools = null;
    public Pool[] Pools
    {
        get { return pools; }
        private set { pools = value; }
    }


    [SerializeField] private Transform _parentObject;

    private void Awake()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledObjects = new List<GameObject>();

            for (int i = 0; i < pools[j].poolSize; i++)
            {
                GameObject obj = Instantiate(pools[j].objectPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(_parentObject, true);
                pools[j].pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(int objectType)
    {
        if (objectType >= pools.Length)
        {
            return null;
        }

        GameObject obj = pools[objectType].pooledObjects[0];
        obj.SetActive(false);
        pools[objectType].pooledObjects.Remove(obj);
        pools[objectType].pooledObjects.Add(obj);
        obj.SetActive(true);

        

        return obj;
        
    }

    public void SetPassiveAllObjects()
    {
        for (int i = 0; i < Pools.Length; i++)
        {
            GameObject obj = pools[i].pooledObjects[0];
            obj.SetActive(false);
        }
    }
}

