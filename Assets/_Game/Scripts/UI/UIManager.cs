using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    private int goldAmount;
    [SerializeField] private TextMeshProUGUI _coinAmountText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject LeftWing;
    [SerializeField] private GameObject RightWing;
    [SerializeField] private GameObject _mobileButtons;
    [SerializeField] private GameObject _inGamePanel;

    private float _score;
    private float _scoreMult = 10;

    public bool _gameOver;

    public float _savedTimeScale;

    [Header("Buttons")]
    [SerializeField] private GameObject _soundOn;
    [SerializeField] private GameObject _soundOff;
    [SerializeField] private GameObject _musicOn;
    [SerializeField] private GameObject _musicOff;

    [Header("Sliders")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    private void OnEnable()
    {
        EventHolder.CoinCollect += IncreaseCoin;
        EventHolder.PlayerDeath += GameOver;
        EventHolder.DoubleBonus += IncreasesScoreMult;
        EventHolder.LeftWingCollected += OpenLeftWing;
        EventHolder.RightWingCollected += OpenRightWing;
    }

    private void OnDisable()
    {
        EventHolder.CoinCollect -= IncreaseCoin;
        EventHolder.PlayerDeath -= GameOver;
        EventHolder.DoubleBonus -= IncreasesScoreMult;
        EventHolder.LeftWingCollected -= OpenLeftWing;
        EventHolder.RightWingCollected -= OpenRightWing;

    }

    private void Start()
    {
        if(MobileDetector.DetectMobile())
        {
            _mobileButtons.SetActive(true);
        }

        if(PlayerPrefs.GetInt("Music") == 0)
        {
            _musicOn.SetActive(false);
            _musicOff.SetActive(true);
        }
        if(PlayerPrefs.GetInt("Sound") == 0)
        {
            _soundOn.SetActive(false);
            _soundOff.SetActive(true);
        }
    }
    private void IncreaseCoin()
    {
        goldAmount++;
        _coinAmountText.text = goldAmount.ToString();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _gameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!_gameOver)
        {
            _score += Time.deltaTime * _scoreMult;
            int score = (int)_score;
            _scoreText.text = score.ToString();
        }

    }

    
    public void IncreasesScoreMult()
    {
        StartCoroutine(IncreasesScoreMultEnumurator());
    }

    IEnumerator IncreasesScoreMultEnumurator()
    {
        _scoreText.color = Color.cyan;
        _scoreMult *= 2;
        yield return new WaitForSeconds(10);
        _scoreMult /= 2;
        _scoreText.color = Color.white;
    }

    public void ChangeScoreColorAtEnd()
    {
        _scoreText.color = Color.grey;
    }
    
    public void OpenLeftWing()
    {
        LeftWing.SetActive(true);
    }

    public void OpenRightWing()
    {
        RightWing.SetActive(true);  
    }


    public void StopTime()
    {
        _savedTimeScale = Time.timeScale;

        Time.timeScale = 0.05f;
    }

    public void RepairTime()
    {
        Time.timeScale = _savedTimeScale;//(PlayerEventPerformer.Instance.PhaseCounter - 1) * 0.2f + 1;
    }


    public float GetMusicSliderValue()
    {
        return _musicSlider.value;
    }

    public void SetMusicSliderValue(float volume)
    {
        _musicSlider.value = volume;
    }

    public float GetSfxSliderValue()
    {
        return _sfxSlider.value;
    }

    public void SetSfxSliderValue(float volume)
    {
        _sfxSlider.value = volume;  
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SetActiveFalseInGamePanel()
    {
        if(!MobileDetector.DetectMobile())
        {
            _inGamePanel.SetActive(false);
        }
    }
}
