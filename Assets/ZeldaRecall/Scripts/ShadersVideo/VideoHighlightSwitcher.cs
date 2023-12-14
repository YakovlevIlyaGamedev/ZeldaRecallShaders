using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoHighlightSwitcher : MonoBehaviour
{
    public Material material1;
    public Material material2;

    public MeshRenderer _renderer;

    float step = 0.7f;

    private void OnValidate()
    {
        _renderer??= GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
            _renderer.material = material1;

        if(Input.GetKeyUp(KeyCode.S))
            _renderer.material = material2;
    }
}
