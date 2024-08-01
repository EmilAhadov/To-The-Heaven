using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private GroundSpawner _groundSpawner;
    [SerializeField] private BuildingSpawner _buildingSpawner;
    [SerializeField] private DecorationSpawner _decorationSpawner;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    public CollectablesSpawner _collectablesSpawner;
    // Start is called before the first frame update

    private bool _coroutineTest = true;

    private float _coolDownTime;

    private bool obstacleTest = true;

    private bool decorTest;

    private float _previousObstacleHorizontalValue;
    private float _currentObstacleHorizontalValue;

    private List<int> goldAmounts = new List<int>();
    private int lastGoldAmountsVoid;

    private int maxPassValue;
    public enum levelState
    {
        levelState1,
        levelState2,
        levelState3,
        levelState4,
        levelState5,
        levelState6
    }

    public levelState _levelState = levelState.levelState1;


    private void OnEnable()
    {
        EventHolder.OnStartNextStepOfLevel += StartNextStep;
        EventHolder.ChangeLevelState += SetLevelState;
    }

    private void OnDisable()
    {
        EventHolder.OnStartNextStepOfLevel -= StartNextStep;
        EventHolder.ChangeLevelState -= SetLevelState;
    }

    private void Start()
    {
        StartSpawning();
    }

    private void StartSpawning()
    {

        StartCoroutine(SwitchGroundMode(StartSpawning));
        
    }
    private void SwitchGroundState()
    {
        _groundSpawner._isSpawningGround = !_groundSpawner._isSpawningGround;
        //_groundSpawner._isSpawningGround = false;

    }

    private bool testing = false;
    //private int testingNumber = 0;

    private IEnumerator SwitchGroundMode(Action onComplete = null)
    {
        _coroutineTest = false;
        //float divisionValue = Time.timeScale;
        _coolDownTime = Random.Range(10, 20);
        for (int i = 0; i < _coolDownTime; i++)
        {
            yield return new WaitForSeconds(1.01f);
            int obstacleGambleVariable = Random.Range(0, 6);

            if ((obstacleGambleVariable > 1 && !obstacleTest) || maxPassValue >= 3)
            {
                maxPassValue = 0;
                _obstacleSpawner.SpawnObstacle(new Vector2(_groundSpawner.transform.position.x, _obstacleSpawner.transform.position.y));
                _currentObstacleHorizontalValue = _groundSpawner.transform.position.x + 6;
                obstacleTest = true;

                if (testing)
                {
                    Debug.Log("Gold iteration work");

                    float obstacleDistance = _currentObstacleHorizontalValue - _previousObstacleHorizontalValue;

                    if (obstacleDistance > 19 && obstacleDistance < 60)
                    {
                        float maxGoldAmount = obstacleDistance / _collectablesSpawner.GoldDistance;
                        int goldIteration = ((int)maxGoldAmount / 10) - 1;


                        for (int j = 0; j < goldIteration; j++)
                        {
                            int goldCount = Random.Range(4, 7);
                            goldAmounts.Add(goldCount);
                        }

                        
                        lastGoldAmountsVoid = 10 - goldAmounts[goldAmounts.Count - 1];
                        _previousObstacleHorizontalValue += lastGoldAmountsVoid / 2;


                        bool isJustOneNonCoinPerTurn = true;
                        for (int j = 0; j < goldIteration; j++)
                        {
                            _collectablesSpawner.transform.position = new Vector2(_previousObstacleHorizontalValue, _collectablesSpawner.transform.position.y);
                            _previousObstacleHorizontalValue += 10;

                            float _collectableGambleVariable = Random.Range(0, 100);
                            Debug.Log($"Gold iteration is {goldIteration}");
                            if (_collectableGambleVariable < 70 || (goldIteration == 1) || !isJustOneNonCoinPerTurn)
                            {
                                _collectablesSpawner.SpawnGold(goldAmounts[j]);
                            }
                            else
                            {
                                if (j != 0)
                                {
                                    _collectablesSpawner.transform.position = new Vector2(_collectablesSpawner.transform.position.x + 5, _collectablesSpawner.transform.position.y);
                                }
                                if (isJustOneNonCoinPerTurn)
                                {
                                    _collectablesSpawner.SpawnRandomSpecialCollectable();
                                    isJustOneNonCoinPerTurn = false;
                                }
                            }
                        }
                        isJustOneNonCoinPerTurn = true;
                    }
                }

                testing = true;
            }
            else
            {
                maxPassValue++;
                obstacleTest = false;
                _previousObstacleHorizontalValue = _currentObstacleHorizontalValue;
                //_collectablesSpawner._spawnGold = false;
            }


            int decorGambleVariable = Random.Range(0, 6);

            if (decorGambleVariable > 3 && !decorTest)
            {
                _decorationSpawner.SpawnDecor(new Vector2(_groundSpawner.transform.position.x, _decorationSpawner.transform.position.y));
            }
            else
            {
                decorTest = false;
            }
        }


        //_obstacleSpawner._spawnObstacle = false;
        SwitchGroundState();
        _coolDownTime = Random.Range(4, 7);
        yield return new WaitForSeconds(_coolDownTime);
        //_obstacleSpawner._spawnObstacle = true;

        SwitchGroundState();

        _coolDownTime = Random.Range(3, 4);
        yield return new WaitForSeconds(_coolDownTime);


        CreateBuilding();
        Debug.Log("Waiting begin");
        _coroutineTest = true;

        yield return new WaitUntil((_buildingSpawner.IsBuildingSpawning));
        yield return new WaitForSeconds(6);



        onComplete?.Invoke();
        Debug.Log("Waiting end");

    }


    private void StartNextStep()
    {

        StartCoroutine(Wait(100f));

        //_coroutineTest = true;
        testing = false;
    }
    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);

    }
    void CreateBuilding()
    {
        _buildingSpawner.gameObject.transform.position = new Vector2(_groundSpawner.transform.position.x, _buildingSpawner.transform.position.y);
        _buildingSpawner._spawnFirstFloor = true;
        _buildingSpawner._spawnSecondFloor = true;
    }

    public void SetLevelState(int index)
    {
        switch (index)
        {
            case 2:
                Time.timeScale = 1.2f;
                _levelState = levelState.levelState2;
                break;
            case 3:
                Time.timeScale = 1.4f;
                _levelState = levelState.levelState3;
                break;
            case 4:
                Time.timeScale = 1.6f;
                _levelState = levelState.levelState4;
                break;
            case 5:
                Time.timeScale = 1.8f;
                _levelState = levelState.levelState5;
                break;
            case 6:
                Time.timeScale = 2f;
                _levelState = levelState.levelState6;
                break;
            default:
                break;
        }
    }


    public void CreateObstacleWay()
    {

    }
}
