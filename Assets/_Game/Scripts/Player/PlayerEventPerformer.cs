using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventPerformer : MonoSingleton<PlayerEventPerformer>
{
    [SerializeField] private AbilityStrategy[] _abilityStrategies;
    private SpriteRenderer _spriteRenderer;
    private bool _isTakeDamage;
    private bool _isForceFieldActive;

    [SerializeField] private GameObject _forceField;

    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private bool _immortalMode;
    public bool ImmortalMode { private get { return _immortalMode; } set { _immortalMode = value; } }

    private bool _workOnceHeightTest = true;

    [SerializeField] private int _phaseCounter;
    public int PhaseCounter { get { return _phaseCounter; } private set { } }


    public bool _isFirstFinishItemTaken;
    public bool _isSecondFinishItemTaken;

    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;

    public GameObject _fireBallThing;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();  
        _boxCollider = GetComponent<BoxCollider2D>();

    }

    private void OnEnable()
    {
        EventHolder.PerformDamage += TakeDamage;
        EventHolder.TriggerAbility += ActivateAbility;
        EventHolder.ActivateShield += ActivateShield;
        EventHolder.ActivateFireBall += ShotFireball;
        EventHolder.DeactivateShield += DeactivateShiel;
        EventHolder.PauseGame += StopPlayer;
        EventHolder.ResumeGame += RunAgain;
    }

    private void OnDisable()
    {
        EventHolder.PerformDamage -= TakeDamage;
        EventHolder.TriggerAbility -= ActivateAbility;
        EventHolder.ActivateShield -= ActivateShield;
        EventHolder.ActivateFireBall -= ShotFireball;
        EventHolder.DeactivateShield -= DeactivateShiel;
        EventHolder.PauseGame -= StopPlayer;
        EventHolder.ResumeGame -= RunAgain;
    }

    private void Update()
    {
        //Mathf.Clamp(transform.position.y, 26, 100);
        PerformTakenDamage();

        if (transform.position.y < -13 /*&& _workOnceHeightTest*/)
        {
            EventHolder.PerformDamage -= SoundManager.Instance.MakeHurtSound;
            EventHolder.Instance.OnPerformDamage();
            //_workOnceHeightTest = false;
            transform.position = new Vector2(0, 100f);
            EventHolder.PerformDamage += SoundManager.Instance.MakeHurtSound;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coin":
                if (PlayerEventPerformer.Instance._isFirstFinishItemTaken && PlayerEventPerformer.Instance._isSecondFinishItemTaken)
                {

                }
                else
                {
                    EventHolder.Instance.OnCoinCollect();
                    collision.gameObject.SetActive(false);
                }
                break;
            case "Shield":
                if (PlayerEventPerformer.Instance._isFirstFinishItemTaken && PlayerEventPerformer.Instance._isSecondFinishItemTaken)
                {

                }
                else
                {
                    EventHolder.Instance.OnTriggerAbility(0);
                    collision.gameObject.SetActive(false);
                }
                break;
            case "FireBall":
                if (PlayerEventPerformer.Instance._isFirstFinishItemTaken && PlayerEventPerformer.Instance._isSecondFinishItemTaken)
                {

                }
                else
                {
                    EventHolder.Instance.OnTriggerAbility(1);
                    SoundManager.Instance.MakeGetFireBallSound();
                    _fireBallThing.SetActive(true);
                    collision.gameObject.SetActive(false);
                }
                break;
            case "2X":
                if (PlayerEventPerformer.Instance._isFirstFinishItemTaken && PlayerEventPerformer.Instance._isSecondFinishItemTaken)
                {

                }
                else
                {
                    EventHolder.Instance.OnDoubleBonus();
                    collision.gameObject.SetActive(false);
                }
                break;
            case "HealthPotion":
                if (PlayerEventPerformer.Instance._isFirstFinishItemTaken && PlayerEventPerformer.Instance._isSecondFinishItemTaken)
                {

                }
                else
                {
                    EventHolder.Instance.OnHeal();
                    HealthSystem.Instance.HealDamage(collision.GetComponent<HealthPotion>().HealthValue);
                    collision.gameObject.SetActive(false);
                }
                break;
            case "Obstacle":
                if(!_immortalMode)
                {
                    if (_isForceFieldActive)
                    {
                        EventHolder.Instance.OnDeactivateShield();

                    }
                    else
                    {
                        EventHolder.Instance.OnPerformDamage();
                    }
                }
                break;
            case "Border":
                _phaseCounter++;
                EventHolder.Instance.OnChangeLevelState(_phaseCounter);
                break;
            case "LeftWing":
                _isFirstFinishItemTaken = true;
                EventHolder.Instance.OnLeftWingCollected();
                collision.gameObject.SetActive(false);
                break;

            case "RightWing":
                _isSecondFinishItemTaken = true;
                _immortalMode = true;
                EventHolder.Instance.OnRightWingCollected();
                UIManager.Instance._gameOver = true;
                UIManager.Instance.ChangeScoreColorAtEnd();
                collision.gameObject.SetActive(false);
                StartCoroutine(Wait(10f));
                Time.timeScale = 1.0f;
                //EventHolder.Instance.OnDeactivateShield();
                _forceField.gameObject.SetActive(false);
                _fireballPrefab.gameObject.SetActive(false);
                GameObject obj = BuildingSpawner.Instance.ObjectPool.GetPooledObject(7);
                obj.transform.position = new Vector2(PlayerEventPerformer.Instance.transform.position.x + 250, 70);
                break;
            case "Finish":
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                camera.GetComponent<CinemachineVirtualCamera>().Follow = null;
                camera.GetComponent<CinemachineVirtualCamera>().LookAt = null;
                break;

            case "ReplayScreen":
                HealthSystem.Instance.TakeDamage(200f);
                break;

            default:
                break;
        }
    }

    
    public void TakeDamage()
    {
        float damageValue = 0;
        if (transform.position.y > -10)
        {
            damageValue = Random.Range(15, 30);
        }
        else
        {
            damageValue = 101;
        }
        HealthSystem.Instance.TakeDamage(damageValue);
        _isTakeDamage = true;
    }

    public void PerformTakenDamage()
    {
        if (_isTakeDamage)
        {
            StartCoroutine(PerformTakenDamage1());
        }
    }

    private IEnumerator PerformTakenDamage1()
    {
        Debug.Log("Change Color");
        _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.red, 10f * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.white, 10f * Time.deltaTime);
        _isTakeDamage = false;
        
    }

    private void ActivateAbility(int index)
    {
        _abilityStrategies[index].UseAbility(transform);
    }

    public void ActivateShield()
    {
        _isForceFieldActive = true;
        _forceField.SetActive(true);
    }

    public void DeactivateShiel()
    {
        _isForceFieldActive = false;
        _forceField.SetActive(false);
    }

    private void ShotFireball()
    {
        Instantiate(_fireballPrefab, new Vector2(transform.position.x + 3, transform.position.y), Quaternion.identity);
        _fireballPrefab.SetActive(true);
    }

    public void StopPlayer()
    {
        PlayerController.Instance.Speed = 0;    
    }

    public void RunAgain()
    {
        PlayerController.Instance.Speed = 10;
    }


    //public void OnRetry()
    //{
    //    _workOnceHeightTest = true;
    //}



    public IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        _boxCollider.isTrigger = true;
    }



}
