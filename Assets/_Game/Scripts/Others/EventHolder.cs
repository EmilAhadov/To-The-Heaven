using System;
using UnityEngine;

public class EventHolder : MonoSingleton<EventHolder>
{
    public static event Action PerformDamage;
    public static event Action OnStartNextStepOfLevel;
    public static event Action CoinCollect;
    public static event Action PlayerDeath;
    public static Action<int> TriggerAbility;
    public static Action ActivateShield;
    public static Action DeactivateShield;
    public static Action ActivateFireBall;
    public static Action DoubleBonus;
    public static event Action<float> IncreaseHealth;
    public static event Action Heal;
    public static event Action Fall;
    public static event Action<int> ChangeLevelState;
    public static event Action LeftWingCollected;
    public static event Action RightWingCollected;
    public static event Action PauseGame;
    public static event Action ResumeGame;
    //public static event Action PhaseChange;


    public void OnPerformDamage()
    {
        if (PerformDamage != null)
            PerformDamage();
    }

    public void StartNextStepOfLevel()
    {
        if(OnStartNextStepOfLevel != null)
            OnStartNextStepOfLevel();
    }

    public void OnCoinCollect()
    {
        if(CoinCollect != null)
            CoinCollect();
    }

    public void OnPlayerDeath()
    {
        if(PlayerDeath != null) 
            PlayerDeath();
    }

    public void OnTriggerAbility(int index)
    {
        if (TriggerAbility != null)
            TriggerAbility(index);
    }

    public void OnActivateShield()
    {
        if(ActivateShield != null)
            ActivateShield();
    }

    public void OnDeactivateShield()
    {
        if(DeactivateShield != null)
            DeactivateShield();
    }

    public void OnActivateFireBall()
    {
        if(ActivateFireBall != null) 
            ActivateFireBall();
    }

    public void OnHeal()
    {
        if(Heal != null)
            Heal();
    }
    
    public void OnFall()
    {
        if(Fall != null)
            Fall();
    }

    public void OnDoubleBonus()
    {
        if(DoubleBonus != null)
            DoubleBonus();
    }

    public void OnChangeLevelState(int index)
    {
        if (ChangeLevelState != null)
            ChangeLevelState(index);
    }

    public void OnLeftWingCollected()
    {
        if(LeftWingCollected != null)
            LeftWingCollected();    
    }

    public void OnRightWingCollected()
    {
        if(RightWingCollected != null)
            RightWingCollected();
    }

    public void OnPauseGame()
    {
        if(PauseGame != null)
            PauseGame();
    }

    public void OnResumeGame()
    {
        if(ResumeGame != null)
            ResumeGame();
    }

    //public void OnPhaseChange()
    //{
    //    if(PhaseChange != null) 
    //        PhaseChange();
    //}

}
