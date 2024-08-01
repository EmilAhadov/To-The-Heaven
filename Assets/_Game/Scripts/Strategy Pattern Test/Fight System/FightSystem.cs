using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSystem : MonoBehaviour 
{
    [SerializeField] private List<GameObject> _enemiesObjects;
    private List<Enemy> _enemies = new List<Enemy>();

    [SerializeField] private Weapons _currentWeaponObject;

    private bool _playerTeamTurn = true;


    private void Start()
    {
        foreach (GameObject enemy in _enemiesObjects)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            _enemies.Add(newEnemy);
        }
    }

    private void Update()
    {
        if(_playerTeamTurn)
        {
            if(Input.GetKeyDown(KeyCode.H)) 
            {
                foreach (Enemy enemy in _enemies)
                {
                    if (enemy != null)
                    {
                        enemy.TakeDamage(_currentWeaponObject._currentDamage);
                    }
                }
            }


        }
    }
}