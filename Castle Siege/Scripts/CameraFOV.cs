using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraFOV: MonoBehaviour
{

    [SerializeField] float _levelSize = 10;
    Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSize();
    }

    private void ChangeSize()
    {
        float widthDiff = _levelSize / Screen.width;
        //Portrait => float heightDiff = _levelSize / Screen.height;

        float cameraSize = 0.5f * widthDiff * Screen.height;
        //Portrait => float cameraSize = 0.5f * heightDiff * Screen.width;

        if (_camera.orthographic)
        {
            _camera.orthographicSize = cameraSize;
        }
        else
        {
            _camera.fieldOfView = cameraSize;
            //Seria mejor modificar el valor de z de la camara en vez del field of view
        }
    }
}
