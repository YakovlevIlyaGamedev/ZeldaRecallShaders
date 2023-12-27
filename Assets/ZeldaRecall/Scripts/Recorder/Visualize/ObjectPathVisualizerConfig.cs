using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPathVisualizerConfig", menuName = "Spells/ObjectPathVisualizerConfig")]
public class ObjectPathVisualizerConfig : ScriptableObject
{
    [field: SerializeField] public LineRenderer PathPrefab { get; private set; }
    [field: SerializeField] public GameObject ArrowHeadPrefab { get; private set; }

    [field: SerializeField, Range(10, 200)] public int IntervalBetweenVisualisableObjects { get; private set; } = 15;
}
