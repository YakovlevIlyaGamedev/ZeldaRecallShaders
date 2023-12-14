using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSreenEffect : MonoBehaviour
{
    [SerializeField] private GrayscaleScreenEffect _grayscaleScreenEffect;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
            _grayscaleScreenEffect.Show();

        if (Input.GetKeyUp(KeyCode.Z)) 
            _grayscaleScreenEffect.Hide();  
    }
}
