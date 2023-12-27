using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour, ITransformable
{
    public event Action<Vector3> MovementDirectionComputed;

    [SerializeField, Range(0, 15)] private float _speed;

    private const float FullRotation = 360;

    private CharacterInput _input;
    private CharacterController _characterController;

    private Camera _camera;

    private float _currentTargetRotation;
    private float _timeToReachTargetRotation = 0.14f;
    private float _dampedTargetRotationCurrentVelocity;
    private float _dampedTargetRotationPassedTime;

    public Transform Transform => transform;

    [Inject]
    private void Construct(CharacterInput input)
    {
        _input = input;
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
    }

    private void OnEnable() => _input.Enable();

    private void OnDisable() => _input.Enable();

    private void Update()
    {
        Vector2 inputDirection = ReadMovementInput();
        Vector3 convertedDirection = GetConvertedInputDirection(inputDirection);

        MovementDirectionComputed?.Invoke(convertedDirection);

        float inputAngleDirection = GetDirectionAngleFrom(convertedDirection);
        inputAngleDirection = AddCameraAngleTo(inputAngleDirection);

        if(convertedDirection != Vector3.zero)
        {
            Rotate(inputAngleDirection);
            Move(Quaternion.Euler(0, inputAngleDirection, 0) * Vector3.forward);
        }
    }

    private void Rotate(float inputAngleDirection)
    {
        if (inputAngleDirection != _currentTargetRotation)
            UpdateTargetRotationData(inputAngleDirection);

        RotateTowardsTargetRotation();
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        _currentTargetRotation = targetAngle;
        _dampedTargetRotationPassedTime = 0f;
    }

    private void RotateTowardsTargetRotation()
    {
        float currentYAngle = GetCurrentRotationAngle();

        if (currentYAngle == _currentTargetRotation)
            return;

        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, _currentTargetRotation, ref _dampedTargetRotationCurrentVelocity, _timeToReachTargetRotation - _dampedTargetRotationPassedTime);
        _dampedTargetRotationPassedTime += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0, smoothedYAngle, 0);
        transform.rotation = targetRotation;
    }

    private float GetCurrentRotationAngle() => transform.rotation.eulerAngles.y;

    private float AddCameraAngleTo(float angle)
    {
        angle += _camera.transform.eulerAngles.y;

        if(angle > FullRotation)
            angle -= FullRotation;
  
        return angle;
    }

    private float GetDirectionAngleFrom(Vector3 inputMoveDirection)
    {
        float directionAngle = Mathf.Atan2(inputMoveDirection.x, inputMoveDirection.z) * Mathf.Rad2Deg;

        if (directionAngle < 0)
            directionAngle += FullRotation;

        return directionAngle;
    }

    private float GetScaledMoveSpeed() => _speed * Time.deltaTime;

    private void Move(Vector3 inputDirection)
    {
        float scaledMoveSpeed = GetScaledMoveSpeed();

        Vector3 normalizedInputDirection = inputDirection.normalized;

        _characterController.Move(normalizedInputDirection * scaledMoveSpeed);
    }

    private Vector2 ReadMovementInput() => _input.Movement.Move.ReadValue<Vector2>();

    private Vector3 GetConvertedInputDirection(Vector2 direction) => new Vector3(direction.x, 0, direction.y);
}
