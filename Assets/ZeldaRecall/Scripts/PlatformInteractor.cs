using UnityEngine;

public class PlatformInteractor : MonoBehaviour
{
    private bool _isEmpty = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterMover character) && _isEmpty)
        {
            _isEmpty = false;
            character.GetComponent<GravityHandler>().enabled = false;
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterMover character) && _isEmpty == false)
        {
            _isEmpty = true;
            character.GetComponent<GravityHandler>().enabled = true;
            other.transform.SetParent(null);
        }
    }
}
