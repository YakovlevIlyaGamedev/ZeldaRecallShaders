using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterView : MonoBehaviour
{
    private const string MovementDirectionKey = "MovementDirection";
    private const string RecallSpellCastKey = "RecallSpellCast";

    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private ParticleSystem _energySparks;
    private Animator _animator;

    private void Awake() => _animator = GetComponent<Animator>();

    private void OnEnable()
    {
        _characterMover.MovementDirectionComputed += OnMovementDirectionComputed;
    }

    private void OnDisable()
    {
        _characterMover.MovementDirectionComputed -= OnMovementDirectionComputed;
    }

    private void OnMovementDirectionComputed(Vector3 movementDirection) => _animator.SetFloat(MovementDirectionKey, movementDirection.magnitude);

    public void RecallSpellPoseActivate()
    {
        _animator.SetBool(RecallSpellCastKey, true);
        _energySparks.Play();
    }

    public void RecallSpellPoseDeactivate()
    {
        _animator.SetBool(RecallSpellCastKey, false);
        _energySparks.Stop();
    }
}
