using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Aim : ITickable
{
    public event Action<Vector2> PositionChanged; 
    public event Action<bool> TargetDetected; 
    public event Action Enabled;
    public event Action Disabled;

    private CharacterInput _input;

    private IEnumerable<RecallInteractor> _availableTargets;
    private RecallInteractor _currentTarget;

    private Camera _camera;

    public Aim(CharacterInput input)
    {
        _input = input;
        _camera = Camera.main;
    }

    public RecallInteractor CurrentTarget => _currentTarget;

    public bool IsEnable { get; private set; }

    public void Tick()
    {
        if (IsEnable == false)
            return;

        Debug.Log(CurrentTarget);

        Vector2 mousePosition = ReadInputPosition();

        PositionChanged?.Invoke(mousePosition);

        if(TryGetTargetFrom(mousePosition, out RecallInteractor target))
        {
            if (_currentTarget == null)
            {
                DetectTarget(target);
                return;
            }

            if (_currentTarget != target)
            {
                DetectTarget(target);
                return;
            }
        }
        else
        {
            if (_currentTarget == null)
                return;

            UndetectedTarget();
        }
    }

    public void Enable(IEnumerable<RecallInteractor> availableTargets)
    {
        _availableTargets = availableTargets;

        IsEnable = true;
        Enabled?.Invoke();
    }

    public void Disable()
    {
        _availableTargets = null;
        _currentTarget = null;

        IsEnable = false;
        Disabled?.Invoke();
    }

    private bool TryGetTargetFrom(Vector2 mousePosition, out RecallInteractor target)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            if (hit.collider.TryGetComponent(out target))
                if(_availableTargets.Contains(target))
                    return true;

        target = null;
        return false;
    }

    private void DetectTarget(RecallInteractor target)
    {
        _currentTarget = target;
        TargetDetected?.Invoke(true);
    }

    private void UndetectedTarget()
    {
        _currentTarget = null;
        TargetDetected?.Invoke(false);
    }

    private Vector2 ReadInputPosition() => _input.CastRecall.AimTarget.ReadValue<Vector2>();
}
