using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RecallSpellMediator: IDisposable
{
    private const float StandardVignetteSmoothness = 0.4f;
    private const float ActivatedVignetteSmoothness = 1f;

    private RecallSpell _recallSpell;
    private CharacterView _characterView;
    private CharacterInput _input;
    private ScannerEffect _scannerEffect;
    private GrayscaleScreenEffect _grayscaleScreenEffect;
    private CameraModeSwitcher _cameraModeSwitcher;
    private TimerView _timerView;
    private Vignette _vignette;

    private ParticleSystem _activateSparksPrefab;

    public RecallSpellMediator(RecallSpell recallSpell, CharacterView characterView, CharacterInput input,
        ScannerEffect scannerEffect, GrayscaleScreenEffect grayscaleScreenEffect, ParticleSystem activateSparksPrefab, 
        CameraModeSwitcher cameraModeSwitcher, TimerView timerView, Volume postProcess)
    {
        _recallSpell = recallSpell;
        _characterView = characterView;
        _input = input;
        _cameraModeSwitcher = cameraModeSwitcher;
        _timerView = timerView;
        _scannerEffect = scannerEffect;
        _grayscaleScreenEffect = grayscaleScreenEffect;
        _activateSparksPrefab = activateSparksPrefab;

        if (postProcess.profile.TryGet<Vignette>(out _vignette) == false)
            throw new ArgumentException(nameof(_vignette));

        _recallSpell.CastStarted += OnCastStarted;
        _recallSpell.CastCanceled += OnCastCanceled;
        _recallSpell.CastApplied += OnCastApplied;
        _recallSpell.CastPerformed += OnCastPerformed;
    }

    private void OnCastStarted()
    {
        _scannerEffect.Activate();

        _characterView.RecallSpellPoseActivate();
        _vignette.smoothness.value = ActivatedVignetteSmoothness;

        _input.Movement.Disable();

        _cameraModeSwitcher.ActivateCastCamera();
    }

    public void Dispose()
    {
        _recallSpell.CastStarted -= OnCastStarted;
        _recallSpell.CastCanceled -= OnCastCanceled;
        _recallSpell.CastApplied -= OnCastApplied;
        _recallSpell.CastPerformed -= OnCastPerformed;

        _scannerEffect.Deactivate();
    }

    private void OnCastApplied(Transform rewindObjectTransform)
    {
        _scannerEffect.Deactivate();
        _grayscaleScreenEffect.Show();

        _characterView.RecallSpellPoseDeactivate();

        _vignette.smoothness.value = StandardVignetteSmoothness;

        _input.Movement.Enable();

        UnityEngine.Object.Instantiate(_activateSparksPrefab, rewindObjectTransform.position, Quaternion.identity, null);

        _timerView.Show(rewindObjectTransform);

        _cameraModeSwitcher.DeactivateCastCamera();
    }

    private void OnCastCanceled()
    {
        _scannerEffect.Deactivate();

        _characterView.RecallSpellPoseDeactivate();

        _vignette.smoothness.value = StandardVignetteSmoothness;

        _input.Movement.Enable();

        _cameraModeSwitcher.DeactivateCastCamera();
    }

    private void OnCastPerformed(Transform rewindObjectTransform)
    {
        _timerView.Hide();
        _grayscaleScreenEffect.Hide();

        UnityEngine.Object.Instantiate(_activateSparksPrefab, rewindObjectTransform.position, Quaternion.identity, null);
    }
}
