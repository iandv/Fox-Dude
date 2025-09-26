using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarWatcher : MonoBehaviour
{
    private Image _powerBar;
    private SuperCounter _superCounter;
    private int _previousNumber = -1;

    void Awake()
    {
        _powerBar = GetComponent<Image>();
        _superCounter = FindObjectOfType<SuperCounter>();
    }

    void Update()
    {
        if (_superCounter.killsMade != _previousNumber)
        {
            _previousNumber = _superCounter.killsMade;
            _powerBar.fillAmount = (float)_superCounter.killsMade / (float)_superCounter.killsToSuper;
        }
    }       
}