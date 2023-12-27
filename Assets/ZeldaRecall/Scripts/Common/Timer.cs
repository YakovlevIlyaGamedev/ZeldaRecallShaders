using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    private IEnumerator _countdown;

    private MonoBehaviour _context;

    public event Action HasBeenUpdated;
    public event Action TimeIsOver;

    public Timer(MonoBehaviour context) => _context = context;

    public float MaxTime { get; private set; }
    public float RemainigTime { get; private set; }

    public void Set(float maxTime, float currentTime)
    {
        if (maxTime < currentTime)
            throw new ArgumentOutOfRangeException(nameof(currentTime));

        MaxTime = maxTime;
        RemainigTime = currentTime;

        InvokeUpdate();
    }

    public void StartCountingTime()
    {
        _countdown = Countdown();
        _context.StartCoroutine(_countdown);
    }

    public void StopCountingTime()
    {
        if(_countdown != null)
            _context.StopCoroutine(_countdown);

        RemainigTime = MaxTime;
    }

    private IEnumerator Countdown()
    {
        while (RemainigTime >= 0)
        {
            RemainigTime -= Time.deltaTime;

            InvokeUpdate();

            yield return null;
        }

        TimeIsOver?.Invoke();
    }

    private void InvokeUpdate() => HasBeenUpdated?.Invoke();
}
