using UnityEngine;

[CreateAssetMenu(fileName = "RecallSpellConfig", menuName = "Spells/RecallSpellConfig")]
public class RecallSpellConfig : ScriptableObject
{
    [SerializeField, Range(0, 10)] private float _secondsToRecord;
    [SerializeField, Range(0, 20)] private float _radius;
    [SerializeField] private string _spellInteractableLayerName;
    [SerializeField] private string _timeRewindLayerName;

    public float SecondsToRecord => _secondsToRecord;
    public LayerMask SpellInteractableLayerMask => 1 << LayerMask.NameToLayer(_spellInteractableLayerName);
    public int SpellInteractableLayer => LayerMask.NameToLayer(_spellInteractableLayerName);
    public int TimeRewindLayer => LayerMask.NameToLayer(_timeRewindLayerName);
    public float Radius => _radius;
}
