using Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraRotator : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private float _sensitivity;

    private CinemachinePOV _cinemachinePOV;

    private CharacterInput _input;

    private float _xAxis;
    private float _yAxis;

    [Inject]
    private void Construct(CharacterInput input) => _input = input;

    private void OnValidate() => _camera ??= GetComponent<CinemachineVirtualCamera>();

    private void Awake() => _cinemachinePOV = _camera.GetCinemachineComponent<CinemachinePOV>();

    private void Update()
    {
        Vector2 inputDirection = _input.Movement.Look.ReadValue<Vector2>();

        _xAxis += inputDirection.x * _sensitivity;
        _yAxis -= inputDirection.y * _sensitivity;

        _cinemachinePOV.m_HorizontalAxis.Value = _xAxis;
        _cinemachinePOV.m_VerticalAxis.Value = _yAxis;
    }
}
