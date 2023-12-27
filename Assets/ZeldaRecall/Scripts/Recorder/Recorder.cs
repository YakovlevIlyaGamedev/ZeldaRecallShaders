using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Recorder<RecordableValue> : MonoBehaviour, IRecorder, IPause
{
    private const int NumberOfSkippedFrames = 5;
    private int _recordFramesCounter = 0;
    private int _rewindFramesCounter = 0;

    public event Action<RecordableValue> InterpolatedValue;
    public event Action RestoredValue;
    public event Action DataHasEnded;

    private RecordableValue _currentValue;
    private RecordableValue _targetValue;

    private RecordBuffer<RecordableValue> _buffer;

    private bool _isRecording;

    private PauseHandler _pauseHandler;
    private bool _isPaused;

    public IReadOnlyList<RecordableValue> RecordableValues => _buffer.RecordableValues;

    [Inject]
    public void Construct(RecallSpellConfig recallSpellConfig, PauseHandler pauseHandler)
    {
        _pauseHandler = pauseHandler;
        _pauseHandler.Add(this);

        float recordsPerSecond = 1 / Time.fixedDeltaTime / NumberOfSkippedFrames;
        int capacity = Mathf.RoundToInt(recallSpellConfig.SecondsToRecord * recordsPerSecond);

        _buffer = new RecordBuffer<RecordableValue>(capacity);
        StartRecord();
    }

    private void OnDestroy() => _pauseHandler.Remove(this);

    private void FixedUpdate()
    {
        if(_isPaused)
            return;

        if (_isRecording)
        {
            if (RecordableValues.Count != 0)
                if (IsDataValuesChanged(_buffer.LastRecordableValue) == false)
                    return;

            if (_recordFramesCounter < NumberOfSkippedFrames)
            {
                _recordFramesCounter++;
            }
            else
            {
                _recordFramesCounter = 0;
                _buffer.Write(GetRecordValue());
            }
        }
        else
        {
            if(_rewindFramesCounter > 0)
            {
                _rewindFramesCounter--;
            }
            else
            {
                _rewindFramesCounter = NumberOfSkippedFrames - 1;
                RestoredValue?.Invoke();
                ReadNewValue();
            }

            float progress = _rewindFramesCounter / (float)NumberOfSkippedFrames;
            RestoreValue(_currentValue, _targetValue, progress);
        }
    }

    private void ReadNewValue()
    {
        if(_buffer.TryReadLastValue(out RecordableValue currentValue))
        {
            _currentValue = currentValue;

            if (RecordableValues.Count > 0)
            {
                _targetValue = _buffer.LastRecordableValue;
            }
        }
        else
        {
            DataHasEnded?.Invoke();
        }
    }

    public virtual void SetPause(bool isPaused) => _isPaused = isPaused;

    public virtual void StartRecord()
    {
        _recordFramesCounter = 0;
        _isRecording = true;
    }

    public virtual void StartRewind()
    {
        _rewindFramesCounter = 0;
        _isRecording = false;
    }

    protected abstract void RestoreValue(RecordableValue currentValue, RecordableValue targetValue, float progress);

    protected virtual void OnInterpolated(RecordableValue value) => InterpolatedValue?.Invoke(value);

    protected abstract RecordableValue GetRecordValue();

    protected abstract bool IsDataValuesChanged(RecordableValue recordableValue);
}
