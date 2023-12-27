using System;
using System.Collections.Generic;

public class RecordBuffer<RecordableValue>
{
    private const int IndexForRecord = 0;

    private List<RecordableValue> _recordableValues;

    private int _capacity;

    public RecordBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        _capacity = capacity;
        _recordableValues = new List<RecordableValue>();
    }

    public IReadOnlyList<RecordableValue> RecordableValues => _recordableValues;
    public RecordableValue LastRecordableValue => _recordableValues[IndexForRecord];

    public void Write(RecordableValue recordableValue)
    {
        if (_capacity <= _recordableValues.Count)
            _recordableValues.RemoveAt(_recordableValues.Count - 1);

        _recordableValues.Insert(IndexForRecord, recordableValue);
    }

    public bool TryReadLastValue(out RecordableValue value)
    {
        if(_recordableValues.Count == 0)
        {
            value = default(RecordableValue);
            return false;
        }

        value = _recordableValues[IndexForRecord];

        _recordableValues.RemoveAt(IndexForRecord);

        return true;
    }
}
