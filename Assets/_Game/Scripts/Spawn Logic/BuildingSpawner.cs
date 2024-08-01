using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSpawner : MonoSingleton<BuildingSpawner>
{
   

    [SerializeField] private float _spawnInterval;
    [SerializeField] private Transform _secondFloorSpawner;

    //Game Objects
    private GameObject obj;

    private bool _isSpawningFirstFloor;

    public bool _spawnFirstFloor = false;
    //public bool IsSpawning
    //{
    //    get { return _isSpawning; }
    //    private set { _isSpawning = value; }
    //}

    private bool _isSpawningSecondFloor;


    //public bool IsSpawningSecondFloor
    //{
    //    get { return _isSpawningSecondFloor; }
    //    private set { _isSpawningSecondFloor = value; }

    //}

    public bool _spawnSecondFloor = false;

    private int _firstFloorBricksAmount;
    private int _secondFloorBricksAmount;
    private int _secondFloorBeginningOffset;

    [SerializeField] private ObjectPool objectPool = null;
    public ObjectPool ObjectPool { get { return objectPool; } private set { } }

    private List<int> goldAmounts = new List<int>();
    private float _firstBrickHorizontalValue;
    private float _lastBrickHorizontlValue;
    private float _secondFloorFirstBrickHorizontalValue;
    private float _secondFloorLastBrickHorizontalValue;
    private int lastGoldAmountsVoid;

    private int _levelIteration;
    public int LevelIteration { get { return _levelIteration; } private set { } }
    private void Awake()
    {
        //StartCoroutine(StartDelay());
        _levelIteration = 0;
    }
    void Start()
    {
        //StartCoroutine(SpawnRoutine());
        _isSpawningFirstFloor = true;
        
        _isSpawningSecondFloor = true;
        SetVariables();
    }


    public bool IsBuildingSpawning()
    {
        return !_spawnFirstFloor; /* || !_spawnSecondFloor;*/
    }

    private void Update()
    {
        SpawnBuilding();
    }

    private void SpawnBuilding()
    {
        if (_spawnFirstFloor)
        {
           FirstFloorSpawn();
        }

        if (_spawnSecondFloor)
        {
            SecondFloorSpawn();
        }
    }

    private void FirstFloorSpawn()
    {
        if(_isSpawningFirstFloor)
        {
            StartCoroutine(FirstFloorSpawnRoutine(_firstFloorBricksAmount));
        }
    }

    private void SecondFloorSpawn()
    {
        if(_isSpawningSecondFloor)
        {
            StartCoroutine(SecondFloorSpawnRoutine(_secondFloorBeginningOffset, _secondFloorBricksAmount));
        }
    }

    private IEnumerator FirstFloorSpawnRoutine(int brickAmount)
    {
        _isSpawningFirstFloor = false;
        yield return new WaitForSeconds(_spawnInterval);

        BuildBrick(0, true);
        transform.position = new Vector2(transform.position.x + 2.55f, transform.position.y);
        _firstBrickHorizontalValue = transform.position.x;

        for (int i = 0; i < brickAmount; i++)
        {
            yield return new WaitForSeconds(_spawnInterval);

            BuildBrick(1, true);
            transform.position = new Vector2(transform.position.x + 2.55f, transform.position.y);
        }

        yield return new WaitForSeconds(_spawnInterval);

        BuildBrick(2, true);
        transform.position = new Vector2(transform.position.x + 2.55f, transform.position.y);
        _lastBrickHorizontlValue = transform.position.x;

        _isSpawningFirstFloor = true;
        SetVariables();
        _spawnFirstFloor = false;



        AddCollectables(_secondFloorFirstBrickHorizontalValue - 10, _secondFloorLastBrickHorizontalValue, 7);

        AddCollectables(_firstBrickHorizontalValue - 5, _lastBrickHorizontlValue, 3.5f);

        AddCollectables(_firstBrickHorizontalValue - 5, _lastBrickHorizontlValue, 0);

        LevelManager.Instance._collectablesSpawner.firstOnceWingPerTurn = true;
        LevelManager.Instance._collectablesSpawner.secondOnceWingTurn = true;




        _levelIteration++;
        Debug.Log($"Level iteration is {_levelIteration}");
        switch (_levelIteration)
        {
            case 1:
                BuildBrick(6, true);
                break;
            case 3:
                BuildBrick(6, true);
                break;
            case 5:
                BuildBrick(6, true);
                break;
            case 6:
                if (PlayerEventPerformer.Instance._isFirstFinishItemTaken)
                {
                    BuildBrick(6, true);
                }
                else
                {
                    _levelIteration--;
                }

                break;
            case 9:
                BuildBrick(6, true);
                break;
            default:
                break;
        }


        EventHolder.Instance.StartNextStepOfLevel();
    }



    private IEnumerator SecondFloorSpawnRoutine(int beginingOffset, int brickAmount)
    {
        _isSpawningSecondFloor = false;
        for (int i = 0; i < beginingOffset; i++)
        {
            yield return new WaitForSeconds(_spawnInterval);
        }

        BuildBrick(3, false);
        _secondFloorFirstBrickHorizontalValue = transform.position.x;

        for (int i = 0; i < brickAmount; i++)
        {
            yield return new WaitForSeconds(_spawnInterval);

            BuildBrick(4, false);
        }

        yield return new WaitForSeconds(_spawnInterval);

        BuildBrick(5, false);
        _secondFloorLastBrickHorizontalValue = transform.position.x;


        _isSpawningSecondFloor = true;

        _spawnSecondFloor = false;
    }



    public void BuildBrick(int index, bool isFirstFloor)
    {
        if (isFirstFloor)
        {
            obj = objectPool.GetPooledObject(index);
            obj.transform.position = transform.position;
        }
        else
        {
            obj = objectPool.GetPooledObject(index);
            obj.transform.position = _secondFloorSpawner.position;
        }
    }

    private void SetVariables()
    {
        _firstFloorBricksAmount = Random.Range(15, 25);
        _secondFloorBeginningOffset = Random.Range(2, 4);
        _secondFloorBricksAmount = Random.Range(10, _firstFloorBricksAmount - _secondFloorBeginningOffset);
    }


    public void AddCollectables(float firstBrickHorizontalValue, float lastBrickHorizontlValue, float itemVerticalOffset)
    {
        float obstacleDistance = lastBrickHorizontlValue - firstBrickHorizontalValue;
        float maxGoldAmount = obstacleDistance / LevelManager.Instance._collectablesSpawner.GoldDistance;
        int goldIteration = ((int)maxGoldAmount / 10) - 1;

        //Debug.Log($"First Brick: {firstBrickHorizontalValue}, " +
        //    $"Last Brick: {lastBrickHorizontlValue}" +
        //    $"Distance: {obstacleDistance}" +
        //    $"Maximum Gold Amount: {maxGoldAmount}" +
        //    $"Gold Iteration: {goldIteration}");
        for (int j = 0; j < goldIteration; j++)
        {
            int goldCount = Random.Range(4, 7);
            goldAmounts.Add(goldCount);
        }

        lastGoldAmountsVoid = 10 - goldAmounts[goldAmounts.Count - 1];
        firstBrickHorizontalValue += lastGoldAmountsVoid / 2;


        bool isJustOneNonCoinPerTurn = true;
        for (int j = 0; j < goldIteration; j++)
        {
            firstBrickHorizontalValue += 10;
            LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(firstBrickHorizontalValue, LevelManager.Instance._collectablesSpawner.transform.position.y);
            

            float _collectableGambleVariable = Random.Range(0, 100);
            if (_collectableGambleVariable < 70 || !isJustOneNonCoinPerTurn)
            {
                LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(LevelManager.Instance._collectablesSpawner.transform.position.x, LevelManager.Instance._collectablesSpawner.transform.position.y + itemVerticalOffset);
                LevelManager.Instance._collectablesSpawner.SpawnGold(goldAmounts[j]);
                LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(LevelManager.Instance._collectablesSpawner.transform.position.x, LevelManager.Instance._collectablesSpawner.transform.position.y - itemVerticalOffset);
            }
            else
            {
                if(j == 0)
                {
                    LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(LevelManager.Instance._collectablesSpawner.transform.position.x + 5, LevelManager.Instance._collectablesSpawner.transform.position.y + itemVerticalOffset);
                    if (isJustOneNonCoinPerTurn)
                    {
                        LevelManager.Instance._collectablesSpawner.SpawnRandomSpecialCollectableForBuilding();
                        isJustOneNonCoinPerTurn = false;
                    }
                    LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(LevelManager.Instance._collectablesSpawner.transform.position.x, LevelManager.Instance._collectablesSpawner.transform.position.y - itemVerticalOffset);
                }
                if (j != 0)
                {
                    //LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(firstBrickHorizontalValue + 5, LevelManager.Instance._collectablesSpawner.transform.position.y + itemVerticalOffset);
                    LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(LevelManager.Instance._collectablesSpawner.transform.position.x + 5, LevelManager.Instance._collectablesSpawner.transform.position.y + itemVerticalOffset);
                    if (isJustOneNonCoinPerTurn)
                    {
                        LevelManager.Instance._collectablesSpawner.SpawnRandomSpecialCollectableForBuilding();
                        isJustOneNonCoinPerTurn = false;
                    }
                    LevelManager.Instance._collectablesSpawner.transform.position = new Vector2(LevelManager.Instance._collectablesSpawner.transform.position.x, LevelManager.Instance._collectablesSpawner.transform.position.y - itemVerticalOffset);
                }
                //LevelManager.Instance._collectablesSpawner.SpawnRandomSpecialCollectable();
            }
        }
    }
}

