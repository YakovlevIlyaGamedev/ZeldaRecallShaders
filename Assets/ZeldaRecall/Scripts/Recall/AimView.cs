using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AimView : MonoBehaviour
{
    private const float MaxFade = 1;
    private const float MinimalFade = 0;

    private Aim _aimTarget;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _aimingLinesParent;
    [SerializeField] private Sprite _noTargetLines;
    [SerializeField] private Sprite _hasTargetLines;

    [SerializeField] private List<Image> _aimingLines;

    [SerializeField, Range(0, 2)] private float _fadeAnimationTime = .2f;

    [Inject]
    private void Construct(Aim aimTarget)
    {
        _aimTarget = aimTarget;

        if(aimTarget.IsEnable)
            Show();
        else
            Hide();
    }

    private void Awake() => _aimingLinesParent.DOSizeDelta(_aimingLinesParent.sizeDelta / 1.4f, .4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

    private void OnEnable()
    {
        _aimTarget.PositionChanged += OnPositionChanged;
        _aimTarget.TargetDetected += OnTargetDetected;
        _aimTarget.Enabled += Show;
        _aimTarget.Disabled += Hide;
    }

    private void OnDisable()
    {
        _aimTarget.PositionChanged -= OnPositionChanged;
        _aimTarget.TargetDetected -= OnTargetDetected;
        _aimTarget.Enabled -= Show;
        _aimTarget.Disabled -= Hide;
    }

    public void Show() => _canvasGroup.DOFade(MaxFade, _fadeAnimationTime);

    public void Hide() => _canvasGroup.DOFade(MinimalFade, _fadeAnimationTime);

    private void OnTargetDetected(bool hasTarget)
    {
        Sprite sprite = hasTarget ? _hasTargetLines : _noTargetLines;

        foreach (Image aimingLine in _aimingLines)
            aimingLine.sprite = sprite;
    }

    private void OnPositionChanged(Vector2 position) => transform.position = position;
}
