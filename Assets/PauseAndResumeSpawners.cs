using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndResumeSpawners : MonoBehaviour
{

    [SerializeField] private GameObject _spawners;

    private void OnEnable()
    {
        EventHolder.PauseGame += DeactivateSpawners;
        EventHolder.ResumeGame += ActivateSpawners;
    }

    private void OnDisable()
    {
        EventHolder.PauseGame -= DeactivateSpawners;
        EventHolder.ResumeGame -= ActivateSpawners;
    }

    public void ActivateSpawners()
    {
        _spawners.SetActive(true);
    }

    public void DeactivateSpawners()
    {
        _spawners.SetActive(false);
    }
}
