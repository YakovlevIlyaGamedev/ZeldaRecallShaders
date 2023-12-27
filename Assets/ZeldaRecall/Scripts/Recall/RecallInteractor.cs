using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RecallInteractorVisualizer))]
public class RecallInteractor : MonoBehaviour
{
    private List<IRecorder> _recorders = new List<IRecorder>();

    [SerializeField] private RecallInteractorVisualizer _visualizer;

    private void OnValidate()
    {
        _visualizer ??= GetComponent<RecallInteractorVisualizer>();

        IRecorder[] recorders = GetComponents<IRecorder>();

        if (recorders.Length == 0)
            throw new InvalidOperationException("no recording components found");

        _recorders = recorders.ToList();
    }

    public void Prepare(int layer)
    {
        _visualizer.ShowPreparation(layer);
    }

    public void Activate(int layer)
    {
        _visualizer.ShowActivation(layer);

        foreach (IRecorder recorder in _recorders)
            recorder.StartRewind();
    }

    public void Deactivate(int layer)
    {
        _visualizer.Hide(layer);

        foreach (IRecorder recorder in _recorders)
            recorder.StartRecord();
    }
}
