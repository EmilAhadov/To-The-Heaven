using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayManager : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private List<Color> _colors;
    [SerializeField] private float _lerpTime;
    private Color _targetColor;
    private int _colorIndex;

    private float _timer;
    private void Start()
    {
        _targetColor = _colors[0];
    }
    void FixedUpdate()
    {
        if(PlayerEventPerformer.Instance._isFirstFinishItemTaken && PlayerEventPerformer.Instance._isSecondFinishItemTaken)
        {
            _light.color = Color.Lerp(_light.color, Color.black, 0.2f * Time.fixedDeltaTime);
        }
        else
        {
            if (_timer <= 0.95f)
            {
                _light.color = Color.Lerp(_light.color, _targetColor, _lerpTime * Time.fixedDeltaTime);
                _timer = Mathf.Lerp(_timer, 1f, _lerpTime * Time.fixedDeltaTime);
            }
            else
            {
                _timer = 0f;
                _colorIndex++;
                _colorIndex = _colorIndex % _colors.Count;
                Debug.Log($"Color change. Color change count is {_colorIndex}");

                
                if(_colorIndex == 4 || _colorIndex == 3)
                {
                    SoundManager.Instance.MakeAmbianceSound(true);
                }
                else
                {
                    SoundManager.Instance.MakeAmbianceSound(false);
                }
                _targetColor = _colors[_colorIndex];
            }
        }
    }
}
