using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrayscaleScreenEffect : MonoBehaviour
{
    private const float MaxEdge = 1.5f;
    private const string EdgeKey = "_Edge";

    [SerializeField] private Material _grayscaleTransition;
    [SerializeField] private Material _separatingLine;

    [SerializeField] private AnimationCurve _showingCurve;
    [SerializeField] private AnimationCurve _hidingCurve;

    [SerializeField] private List<ScriptableRendererFeature> _effectsFeatures;

    private Coroutine _currentAnimation;

    float _progress = 0;

    private void OnDisable()
    {
        foreach (ScriptableRendererFeature rendererFeature in _effectsFeatures)
            rendererFeature.SetActive(false);
    }

    public void Show()
    {
        foreach (ScriptableRendererFeature rendererFeature in _effectsFeatures)
            rendererFeature.SetActive(true);

        if(_currentAnimation != null)
            StopCoroutine(_currentAnimation);

        _currentAnimation = StartCoroutine(ShowTransitionAnimation());
    }

    public void Hide()
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);

        _currentAnimation = StartCoroutine(HideTransitionAnimation());
    }
    
    private IEnumerator ShowTransitionAnimation()
    {
        while(_progress < MaxEdge)
        {
            _progress += Time.deltaTime;

            SetAnimationProgress(_showingCurve, _progress);

            yield return null;
        }
    }

    private IEnumerator HideTransitionAnimation()
    {
        while (_progress > 0)
        {
            _progress -= Time.deltaTime;

            SetAnimationProgress(_hidingCurve, _progress);

            yield return null;
        }

        OnDisable();
    }

    private void SetAnimationProgress(AnimationCurve animationCurve, float progress)
    {
        float edge = animationCurve.Evaluate(progress / MaxEdge) * MaxEdge;
        _grayscaleTransition.SetFloat(EdgeKey, edge);
        _separatingLine.SetFloat(EdgeKey, edge);
    }
} 
