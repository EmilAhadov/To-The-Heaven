using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundSpawner : MonoSingleton<GroundSpawner> 
{
    [SerializeField] private float _spawnInterval;

    public bool _isSpawningGround;
    public bool IsSpawningGround { get { return _isSpawningGround; }  set { _isSpawningGround = value; } }


    private bool _isGroundCoroutineActive;
    private bool _isColumnCoroutineActive;
    [SerializeField] private ObjectPool objectPool = null;

    public ObjectPool ObjectPool { get { return objectPool; } }
    
    [SerializeField] private float columnOffset;
    [HideInInspector] public float groundOffset = 10.17f;

    private float verticalOffset;
    private float horizontalOffset;
    private bool ignoreFirstColumnHorizontalOffset;

    private GameObject _column;

    private Vector2 _lastSpawnedObject;
    public Vector2 LastSpawnedObject { get { return _lastSpawnedObject; } private set { } }

    public enum State
    {
        platformSpawn,
        columnSpawn
    }

    private State _state;
    void Start()
    {
        _isSpawningGround = true;
        _isGroundCoroutineActive = true;
        _isColumnCoroutineActive = true;

        for(int i = 0; i < 10; i++)
        {
            StartCoroutine(GroundSpawnRoutine(0f));
        }
    }

    private IEnumerator GroundSpawnRoutine(float duration)
    {
        _isGroundCoroutineActive = false;
        
        yield return new WaitForSeconds(duration);
        transform.position = new Vector2(transform.position.x + groundOffset, transform.position.y);
        GameObject ground = objectPool.GetPooledObject(0);
        ground.transform.position = transform.position;

        _isGroundCoroutineActive = true;
    }

    private IEnumerator ColumnSpawnRoutine(float duration)
    {
        _isColumnCoroutineActive = false;

        if(_column != null)
        {
            _lastSpawnedObject = _column.transform.position;
        }
        yield return new WaitForSeconds(duration);
        transform.position = new Vector2(transform.position.x + columnOffset, transform.position.y);

        int gambleGambleVariable = Random.Range(0, 100);
        int gambeVariable = 0;
        if (gambleGambleVariable < 90 )
        {
            gambeVariable = Random.Range(1, 4);
        }
        else
        {
            gambeVariable = Random.Range(5, 7);
        }
        
        _column = objectPool.GetPooledObject(gambeVariable);

        switch (LevelManager.Instance._levelState)
        {
            case LevelManager.levelState.levelState1:
                verticalOffset = 3;

                horizontalOffset = 0;
                break;
            case LevelManager.levelState.levelState2:
                verticalOffset = 3;

                horizontalOffset = Random.Range(0, 3);
                break;
            case LevelManager.levelState.levelState3:
                verticalOffset = Random.Range(2, 5);

                horizontalOffset = Random.Range(0, 3);
                break;
            case LevelManager.levelState.levelState4:
                verticalOffset = Random.Range(0, 5);

                horizontalOffset = Random.Range(0, 3);
                break;
            case LevelManager.levelState.levelState5:
                verticalOffset = Random.Range(0, 5);

                horizontalOffset = Random.Range(0, 3);
                break;
            case LevelManager.levelState.levelState6:
                verticalOffset = 3;

                horizontalOffset = 0;
                break;
            default:
                break;
        }
        
        if (ignoreFirstColumnHorizontalOffset)
        {
            horizontalOffset = 0;
        }
        ignoreFirstColumnHorizontalOffset = false;

        _column.transform.position = new Vector2(transform.position.x - horizontalOffset, transform.position.y - verticalOffset);

        _isColumnCoroutineActive = true;
    }

    private void Update()
    {
        if(Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < 100f)
            StateHandler();
    }

   
    private void StateHandler()
    {
        switch (_state)
        {
            case State.platformSpawn:
                
                if (_isGroundCoroutineActive)
                {
                    StartCoroutine(GroundSpawnRoutine(_spawnInterval));
                }

                if(!_isSpawningGround)
                {
                    ignoreFirstColumnHorizontalOffset = true;
                    _state = State.columnSpawn;
                }
                break;
            case State.columnSpawn:

                if (_isColumnCoroutineActive)
                {
                    StartCoroutine(ColumnSpawnRoutine(_spawnInterval));
                }

                if(_isSpawningGround)
                {
                    _state = State.platformSpawn;
                }
                break;
        }
    }
}
