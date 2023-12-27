using System;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    [SerializeField] private CameraModeSwitcher _cameraModeSwitcher;
    [SerializeField] private TimerView _timerView;

    public override void InstallBindings()
    {
        BindInput();
        BindCameraSwitcher();
        BindPause();
        BindTimer();
    }

    private void BindTimer()
    {
        Container.BindInstance(new Timer(this)).AsSingle();
        Container.Bind<TimerView>().FromInstance(_timerView).AsSingle();
    }

    private void BindCameraSwitcher()
    {
        Container.Bind<CameraModeSwitcher>().FromInstance(_cameraModeSwitcher).AsSingle();
    }

    private void BindInput()
    {
        CharacterInput input = new CharacterInput();
        input.Enable();
        input.CastRecall.Disable();

        Container.Bind<CharacterInput>().FromInstance(input).AsSingle();
    }

    private void BindPause()
    {
        Container.BindInterfacesAndSelfTo<PauseHandler>().AsSingle();
    }
}