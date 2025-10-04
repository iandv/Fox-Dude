using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class VirtualJoystick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform rectTransform;
    public RectTransform knob;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
