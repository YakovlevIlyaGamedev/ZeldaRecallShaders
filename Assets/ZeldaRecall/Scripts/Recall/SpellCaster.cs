using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SpellCaster : MonoBehaviour
{
    private RecallSpell _recallSpell;
    private CharacterInput _input;

    [Inject]
    private void Construct(RecallSpell recallSpell, CharacterInput input)
    {
        _recallSpell = recallSpell;
        _input = input;

        _input.CastRecall.CastSpell.started += OnCastSpellKeyPressed;
    }

    private void OnCastSpellKeyPressed(InputAction.CallbackContext obj)
    {
        _input.CastRecall.CastSpell.started -= OnCastSpellKeyPressed;

        _input.CastRecall.Cancel.started += OnCastCanceledKeyPressed;
        _input.CastRecall.Apply.started += OnCastAppliedKeyPressed;

        _recallSpell.StartCast();
    }

    private void OnCastAppliedKeyPressed(InputAction.CallbackContext obj)
    {
        if (_recallSpell.TryApplyCast() == false)
            return;

        _input.CastRecall.Apply.started -= OnCastAppliedKeyPressed;

        _recallSpell.CastPerformed += OnCastPerformed;
    }

    private void OnCastCanceledKeyPressed(InputAction.CallbackContext obj)
    {
        _input.CastRecall.Cancel.started -= OnCastCanceledKeyPressed;
        _input.CastRecall.Apply.started -= OnCastAppliedKeyPressed;

        _recallSpell.CastCanceled += OnCastCanceled;
        _recallSpell.CancelCast();
    }

    private void OnCastCanceled()
    {
        _recallSpell.CastCanceled -= OnCastCanceled;
        _recallSpell.CastPerformed -= OnCastPerformed;
        _input.CastRecall.CastSpell.started += OnCastSpellKeyPressed;
    }

    private void OnCastPerformed(Transform rewindObjectTransform)
    {
        _input.CastRecall.Cancel.started -= OnCastCanceledKeyPressed;
        _recallSpell.CastCanceled -= OnCastCanceled;
        _recallSpell.CastPerformed -= OnCastPerformed;
        _input.CastRecall.CastSpell.started += OnCastSpellKeyPressed;
    }
}
