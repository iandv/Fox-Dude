using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform pointOne;
    [SerializeField]
    private Transform pointTwo;
    [SerializeField]
    private Slider slider;
    private float _previousSliderValue = -1;

    void Update()
    {
        if (slider.value != _previousSliderValue)
        {
            _previousSliderValue = slider.value;
            transform.position = Vector3.Lerp(pointOne.position, pointTwo.position, slider.value);
        }
        
    }
}
