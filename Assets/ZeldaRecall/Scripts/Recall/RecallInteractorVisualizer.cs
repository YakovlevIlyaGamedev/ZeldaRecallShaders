using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RecallInteractorVisualizer : MonoBehaviour
{
    [SerializeField] private ObjectPathVisualizer _objectPathVisualizer;
    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private Material _standardMaterial;
    [SerializeField] private Material _preparedHighlightMaterial;
    [SerializeField] private Material _activeHighlightMaterial;

    private void OnValidate()
    {
        _meshRenderer ??= GetComponent<MeshRenderer>();
    }

    public void ShowPreparation(int layer)
    {
        SetLayer(layer);

        _objectPathVisualizer.ShowPreparation();
        _meshRenderer.material = _preparedHighlightMaterial;
    }

    public void ShowActivation(int layer)
    {
        SetLayer(layer);

        _objectPathVisualizer.ShowActivation(layer);
        _meshRenderer.material = _activeHighlightMaterial;
    }

    public void Hide(int layer)
    {
        SetLayer(layer);

        _objectPathVisualizer.Hide();
        _meshRenderer.material = _standardMaterial;
    }

    private void SetLayer(int layer) => gameObject.layer = layer;
}
