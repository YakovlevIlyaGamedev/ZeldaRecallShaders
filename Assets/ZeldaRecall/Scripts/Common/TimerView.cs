using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TimerView : MonoBehaviour
{
    [SerializeField] private Image _filler;

    private Timer _timer;
    private Transform _trackedTarget;

    private Camera _camera;

    [Inject]
    public void Initialize(Timer timer)
    {
        _timer = timer;
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.position = _camera.WorldToScreenPoint(_trackedTarget.position);
    }

    public void Show(Transform trackedTarget)
    {
        _trackedTarget = trackedTarget;
        _timer.HasBeenUpdated += OnTimerUpdate;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _timer.HasBeenUpdated -= OnTimerUpdate;

        gameObject.SetActive(false);
    }

    private void OnTimerUpdate()
    {
        _filler.fillAmount = _timer.RemainigTime / _timer.MaxTime;
    }
}
