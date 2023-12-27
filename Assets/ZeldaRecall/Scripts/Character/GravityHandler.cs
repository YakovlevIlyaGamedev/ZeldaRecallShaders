using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHandler : MonoBehaviour
{
    [SerializeField] private float _gravity;

    [SerializeField] private CharacterController _characterController;

    private void Update()
    {
        _characterController.Move(new Vector3(0, -_gravity * Time.deltaTime, 0));
    }
}
