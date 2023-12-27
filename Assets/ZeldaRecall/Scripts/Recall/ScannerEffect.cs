using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScannerEffect: IDisposable
{
    private const string PositionKey = "_Position";

    private ScriptableRendererFeature _scannerFeature;
    private Material _material;

    private Transform _target;

    public ScannerEffect(Transform target, Material material, ScriptableRendererFeature scannerFeature)
    {
        _target = target;
        _material = material;
        _scannerFeature = scannerFeature;
    }

    public void Activate()
    {
        _scannerFeature.SetActive(true);
        _material.SetVector(PositionKey, _target.position);
    }

    public void Deactivate()
    {
        _scannerFeature.SetActive(false);
    }

    public void Dispose() => Deactivate();
}
