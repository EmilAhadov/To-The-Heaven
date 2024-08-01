using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;

    [Header("Sounds")]
    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private AudioClip _fireballSound;
    [SerializeField] private AudioClip _forceFieldDeploy;
    [SerializeField] private AudioClip _forceFieldBreak;
    [SerializeField] private AudioClip _heal;
    [SerializeField] private AudioClip _fall;
    [SerializeField] private AudioClip _takeDamage;
    [SerializeField] private AudioClip _bonus;
    [SerializeField] private AudioClip _wing;
    [SerializeField] private AudioClip _rockHit;
    [SerializeField] private AudioClip _getFireBall;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _music;
    [SerializeField] private AudioClip _gameOverPanelMusic;
    [SerializeField] private AudioClip _dayAmbianceSound;
    [SerializeField] private AudioClip _nightAmbianceSound;

    [Header("Stable Players")]
    [SerializeField] private AudioSource _musicPlayer;
    [SerializeField] private AudioSource _ambianceSoundPlayer;



    //private bool _isSoundMuted;
    //public bool IsSoundMuted { get { return _isSoundMuted; } }

    //private bool _isMusicMuted;
    private void Awake()
    {
        _ambianceSoundPlayer.outputAudioMixerGroup = _sfxGroup;
        _musicPlayer.outputAudioMixerGroup = _musicGroup;


        //_ambianceSoundPlayer.clip = _dayAmbianceSound;
        
        //_musicPlayer.clip = _music;
        //_musicPlayer.Play()
        LoadVolume();



        if (PlayerPrefs.GetInt("Music") == 0)
        {
            _musicPlayer.mute = true;
        }
        else
        {
            _musicPlayer.mute = false;
        }

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            _ambianceSoundPlayer.mute = true;
        }
        else
        {
            _ambianceSoundPlayer.mute = false;
        }

        MakeAmbianceSound(false);
    }
    public void PlaySound(AudioClip sound)
    {
        if (!_ambianceSoundPlayer.mute)
        {
            GameObject _soundObject = new GameObject();
            AudioSource _audioSource = _soundObject.AddComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = _sfxGroup;
            _audioSource.PlayOneShot(sound);
        }
    }

    private void OnEnable()
    {
        EventHolder.CoinCollect += MakeCoinSound;
        EventHolder.ActivateFireBall += MakeFireballSound;
        EventHolder.ActivateShield += MakeForceFieldDeploySound;
        EventHolder.DeactivateShield += MakeForceFieldBreakSound;
        EventHolder.Heal += MakeHealSound;
        EventHolder.PerformDamage += MakeHurtSound;
        EventHolder.Fall += MakeFallSound;
        EventHolder.DoubleBonus += MakeBonusSound;
        EventHolder.LeftWingCollected += MakeWingSound;
        EventHolder.RightWingCollected += MakeWingSound;
        EventHolder.PlayerDeath += MakeGameOverPanelMusic;
    }

    private void OnDisable()
    {
        EventHolder.CoinCollect -= MakeCoinSound;
        EventHolder.ActivateFireBall -= MakeFireballSound;
        EventHolder.ActivateShield -= MakeForceFieldDeploySound;
        EventHolder.DeactivateShield -= MakeForceFieldBreakSound;
        EventHolder.Heal -= MakeHealSound;
        EventHolder.PerformDamage -= MakeHurtSound;
        EventHolder.Fall -= MakeFallSound;
        EventHolder.DoubleBonus -= MakeBonusSound;
        EventHolder.LeftWingCollected -= MakeWingSound;
        EventHolder.RightWingCollected -= MakeWingSound;
        EventHolder.PlayerDeath -= MakeGameOverPanelMusic;
    }

    public void MakeCoinSound()
    {
        PlaySound(_coinSound);
    }

    public void MakeFireballSound()
    {
        PlaySound(_fireballSound);
    }

    public void MakeForceFieldDeploySound()
    {
        PlaySound(_forceFieldDeploy);
    }

    public void MakeForceFieldBreakSound()
    {
        PlaySound(_forceFieldBreak);
    }

    public void MakeHealSound()
    {
        PlaySound(_heal);
    }

    public void MakeHurtSound()
    {
        PlaySound(_takeDamage);
    }

    public void MakeFallSound()
    {
        PlaySound(_fall);
    }

    public void MakeBonusSound()
    {
        PlaySound(_bonus);
    }

    public void MakeWingSound()
    {
        PlaySound(_wing);
    }

    public void MakeRockHitSound()
    {
        PlaySound(_rockHit);
    }

    public void MakeGetFireBallSound()
    {
        PlaySound(_getFireBall);
    }

    public void MakeJumpSound()
    {
        PlaySound(_jump);
    }
    

    public void MakeMusic()
    {
        _musicPlayer.clip = _music;
        _musicPlayer.loop = true;
        _musicPlayer.Play();
    }
    public void MakeGameOverPanelMusic()
    {
        _musicPlayer.clip = _gameOverPanelMusic;
        _musicPlayer.Play();
    }

    public void MakeAmbianceSound(bool isNight)
    {
        if (isNight)
        {
            _ambianceSoundPlayer.clip = _nightAmbianceSound;
            _ambianceSoundPlayer.loop = true;
            _ambianceSoundPlayer.Play();
        }
        else
        {
            _ambianceSoundPlayer.clip = _dayAmbianceSound;
            _ambianceSoundPlayer.loop = true;
            _ambianceSoundPlayer.Play();
        }
    }

    public void SwitchSoundSituation()
    {
        _ambianceSoundPlayer.mute = !_ambianceSoundPlayer.mute;
        if (!_ambianceSoundPlayer.mute)
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
        }
    }

    public void SwitchMusicSituation()
    {
        _musicPlayer.mute = !_musicPlayer.mute;
        if (!_musicPlayer.mute)
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
        }
    }



    public void SetMusicVolume()
    {
        float volume = UIManager.Instance.GetMusicSliderValue();
        _musicGroup.audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSoundVolume()
    {
        float volume = UIManager.Instance.GetSfxSliderValue();
        _sfxGroup.audioMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            UIManager.Instance.SetMusicSliderValue(PlayerPrefs.GetFloat("musicVolume"));
        }
        else
        {
            UIManager.Instance.SetMusicSliderValue(1);
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            UIManager.Instance.SetSfxSliderValue(PlayerPrefs.GetFloat("sfxVolume"));
        }
        else
        {
            UIManager.Instance.SetSfxSliderValue(1);
        }

        SetMusicVolume();
    }
}
