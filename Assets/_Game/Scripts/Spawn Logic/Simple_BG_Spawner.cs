using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_BG_Spawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1;

    [SerializeField] private ObjectPool objectPool = null;
    [SerializeField] private float horizontalOffset;

    private bool _spawnObject;

    private GameObject obj2 = null;

    private int i = 0;
    private void Start()
    {
        
        _spawnObject = true;

        for (int j = 0; j <= 5; j++)
        {
            StartCoroutine(SpawnRoutine(0.1f));
        }
    }
    private void Update()
    {
        if(_spawnObject && Vector2.Distance(PlayerController.Instance.transform.position, obj2.transform.position) < 100f)
        {
            StartCoroutine(SpawnRoutine(spawnInterval));
        }
    }

    private IEnumerator SpawnRoutine(float duration)
    {
        _spawnObject = false;

        GameObject obj = objectPool.GetPooledObject(0);
        if (i == 0)
        {
            obj.transform.position = transform.position;
        }
        else
        {
            obj.transform.position = new Vector2(obj2.transform.position.x + horizontalOffset, transform.position.y); 
        }

        obj2 = obj;

        i++;

        
        yield return new WaitForSeconds(duration);

        _spawnObject = true;
    }
}
