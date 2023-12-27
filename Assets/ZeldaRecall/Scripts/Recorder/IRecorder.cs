using System;

public interface IRecorder
{
    event Action RestoredValue;
    event Action DataHasEnded;

    void StartRecord();

    void StartRewind();
}
