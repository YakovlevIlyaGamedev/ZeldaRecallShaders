using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class RecallSpellInstaller : MonoInstaller
{
    [SerializeField] private RecallSpellConfig _recallSpellConfig;
    [SerializeField] private GrayscaleScreenEffect _grayscaleScreenEffect;

    [SerializeField] private Transform _scannerTarget;
    [SerializeField] private Material _scannerMaterial;
    [SerializeField] private ScriptableRendererFeature _scannerFeature;

    [SerializeField] private ParticleSystem _activateSparksPrefab;
    [SerializeField] private ObjectPathVisualizerConfig _objectPathVisualizerConfig;

    [SerializeField] private Volume _postProcess;

    public override void InstallBindings()
    {
        BindEffects();
        BindAim();
        BindSpell();
    }

    private void BindEffects()
    {
        Container.Bind<GrayscaleScreenEffect>().FromInstance(_grayscaleScreenEffect).AsSingle();
        Container.Bind<ParticleSystem>().FromInstance(_activateSparksPrefab).WhenInjectedInto<RecallSpellMediator>();

        Container.BindInterfacesAndSelfTo<ScannerEffect>().AsSingle()
            .WithArguments(_scannerTarget, _scannerMaterial, _scannerFeature);

        Container.Bind<Volume>().FromInstance(_postProcess).AsSingle();

        Container.Bind<ObjectPathVisualizerConfig>().FromInstance(_objectPathVisualizerConfig);
    }

    private void BindAim() => Container.BindInterfacesAndSelfTo<Aim>().AsSingle();

    private void BindSpell()
    {
        Container.Bind<RecallSpell>().AsSingle();
        Container.Bind<RecallSpellConfig>().FromInstance(_recallSpellConfig).AsSingle();
        Container.Bind<RecallSpellMediator>().AsSingle().NonLazy();
    }
}
