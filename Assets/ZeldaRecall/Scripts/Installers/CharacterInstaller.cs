using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private CharacterView _characterView;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CharacterMover>().FromInstance(_characterMover).AsSingle();
        Container.Bind<CharacterView>().FromInstance(_characterView).AsSingle();
    }
}
