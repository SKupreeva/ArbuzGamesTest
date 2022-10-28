using System;
using System.Collections;
using UnityEngine;

// class controls timer coroutine

public class Timer : MonoBehaviour
{
    [Space][Header("Game components")]
    [SerializeField] private LevelManager _levelManager;

    [Space][Header("TimerValue")]
    [SerializeField] private float _timerValueInSec = 20f;

    public Action OnTimerValueChanged;
    public Action OnTimerEnded;

    public float TimerCurrentValue
    {
        get => _timerCurrentValue;
        private set
        {
            if(value == _timerCurrentValue)
            {
                return;
            }
            _timerCurrentValue = value;
            if(value <= 0)
            {
                OnTimerEnded?.Invoke();
            }

            OnTimerValueChanged?.Invoke();
        }
    }

    private float _timerCurrentValue;

    private void OnEnable()
    {
        _levelManager.OnGameEnded += StopAllCoroutines;
        _levelManager.OnNewGameStarted += StartTimer;
    }

    private void StartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(StartCountdown(_timerValueInSec));
    }

    private IEnumerator StartCountdown(float countdownValue = 10)
    {
        TimerCurrentValue = countdownValue;
        while (TimerCurrentValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            TimerCurrentValue--;
        }
    }
}
