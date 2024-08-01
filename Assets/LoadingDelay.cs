using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingDelay : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _loadingPanel;
    private void Update()
    {
        if(_slider.value == _slider.maxValue)
        {
            PlayerController.Instance.Speed = 10;
            SoundManager.Instance.MakeMusic();
            _loadingPanel.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            _slider.value += Time.deltaTime;
        }
    }
}
