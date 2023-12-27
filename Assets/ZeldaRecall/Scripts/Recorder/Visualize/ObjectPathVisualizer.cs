using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TransformRecorder))]
public class ObjectPathVisualizer : MonoBehaviour
{
    private const float Epsilon = 0.01f;
    private const float PercentageOfPointsForPreparationShowing = 50;
    private const int MinCountOfPointsForPreparationShowing = 3;

    [SerializeField] private TransformRecorder _recorder;

    [SerializeField] private GameObject _visualisableObjectPrefab;
    private LineRenderer _pathPrefab;
    private GameObject _arrowHeadPrefab;

    private int _intervalBetweenVisualisableObjects = 15;

    private LineRenderer _path;
    private GameObject _arrowHead;

    private List<GameObject> _visualisableObjectInstances = new List<GameObject>();

    [Inject]
    private void Contsruct(ObjectPathVisualizerConfig config)
    {
        _pathPrefab = config.PathPrefab;
        _arrowHeadPrefab = config.ArrowHeadPrefab;
        _intervalBetweenVisualisableObjects = config.IntervalBetweenVisualisableObjects;
    }

    private void OnValidate()
    {
        _recorder ??= GetComponent<TransformRecorder>();
    }

    private void OnDisable()
    {
        _recorder.RestoredValue -= UpdateLine;
    }

    public void ShowPreparation()
    {
        ClearVisualisableObjects();
        InstantiatePath();

        IReadOnlyList<TransformValues> points = _recorder.RecordableValues;

        if (points.Count < MinCountOfPointsForPreparationShowing)
            return;

        int showingPointsCount = (int)(points.Count * PercentageOfPointsForPreparationShowing / 100f);

        if (showingPointsCount < MinCountOfPointsForPreparationShowing)
            showingPointsCount = MinCountOfPointsForPreparationShowing;

        _path.positionCount = showingPointsCount;

        for (int i = 0; i < showingPointsCount; i++)
            _path.SetPosition(showingPointsCount - 1 - i, points[i].Position);

        InstantiateArrowHead(points[showingPointsCount - 1].Position, points[showingPointsCount - 2].Position);
    }

    public void ShowActivation(int layer)
    {
        ClearVisualisableObjects();
        InstantiatePath();

        IReadOnlyList<TransformValues> points = _recorder.RecordableValues;

        if (points.Count < MinCountOfPointsForPreparationShowing)
            return;

        _path.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            TransformValues point = points[points.Count - 1 - i];

            if (i % _intervalBetweenVisualisableObjects == 0)
            {
                GameObject visualisableObject = Instantiate(_visualisableObjectPrefab, point.Position, point.Rotation, null);
                visualisableObject.gameObject.layer = layer;
                _visualisableObjectInstances.Add(visualisableObject);
            }

            _path.SetPosition(i, point.Position);
        }

        InstantiateArrowHead(points[points.Count - 1].Position, points[points.Count - 2].Position);
        
        _path.gameObject.layer = layer;
        _arrowHead.gameObject.layer = layer;

        _recorder.RestoredValue += UpdateLine;
        _recorder.InterpolatedValue += OnInterpolated;
    }

    public void Hide()
    {
        if(_path != null)
            _path.positionCount = 0;

        ClearVisualisableObjects();
        DestroyPath();
        DestroyArrowHead();

        _recorder.RestoredValue -= UpdateLine;
        _recorder.InterpolatedValue -= OnInterpolated;
    }

    private void InstantiatePath()
    {
        DestroyPath();

        _path = Instantiate(_pathPrefab);
    }

    private void DestroyPath()
    {
        if (_path != null)
        {
            Destroy(_path.gameObject);
            _path = null;
        }
    }

    private void ClearVisualisableObjects()
    {
        foreach (GameObject visualisableObject in _visualisableObjectInstances)
            Destroy(visualisableObject.gameObject);

        _visualisableObjectInstances.Clear();
    }

    private void InstantiateArrowHead(Vector3 spawnPoint, Vector3 directionPoint)
    {
        DestroyArrowHead();

        _arrowHead = Instantiate(_arrowHeadPrefab, spawnPoint, Quaternion.identity, null);
        _arrowHead.transform.forward = spawnPoint - directionPoint;
    }

    private void DestroyArrowHead()
    {
        if (_arrowHead != null)
        {
            Destroy(_arrowHead.gameObject);
            _arrowHead = null;
        }
    }

    private void OnInterpolated(TransformValues value)
    {
        if (_path.positionCount > 0)
            _path.SetPosition(_path.positionCount - 1, value.Position);
    }

    private void UpdateLine()
    {
        IReadOnlyList<TransformValues> points = _recorder.RecordableValues;

        if (_visualisableObjectInstances.Count > 0)
        {
            GameObject lastVisualisableObject = _visualisableObjectInstances[_visualisableObjectInstances.Count - 1];

            if ((points[0].Position - lastVisualisableObject.transform.position).magnitude < Epsilon)
            {
                Destroy(lastVisualisableObject.gameObject);
                _visualisableObjectInstances.Remove(lastVisualisableObject);
            }
        }

        _path.positionCount = points.Count;
    }
}
