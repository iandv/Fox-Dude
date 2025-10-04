using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideView : MonoBehaviour
{
    private Camera _myCamera;
    void Awake()
    {
        _myCamera = GetComponent<Camera>();
    }

    public void HideCamera()
    {
        if (_myCamera.enabled == true)
        {
            _myCamera.enabled = false;
        }
        
        else
        {
            _myCamera.enabled = true;
        }
    }
}
