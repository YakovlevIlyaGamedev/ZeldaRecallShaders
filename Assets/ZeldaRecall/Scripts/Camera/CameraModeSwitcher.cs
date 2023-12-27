using UnityEngine;
using Cinemachine;

public class CameraModeSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _followCamera;
    [SerializeField] private CinemachineVirtualCamera _castCamera;

    public void ActivateCastCamera()
    {
        _followCamera.gameObject.SetActive(false);

        if (_castCamera.TryGetComponent(out CinemachineInputProvider inputProvider))
            inputProvider.enabled = false;
    }

    public void DeactivateCastCamera()
    {
        _followCamera.gameObject.SetActive(true);

        if (_castCamera.TryGetComponent(out CinemachineInputProvider inputProvider))
            inputProvider.enabled = true;
    }
}
