using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileKey : MonoBehaviour
{
    protected KeyCounter _keyCounter;
    protected ChaseBehaviour _chase;
    protected bool _isDead;

    void Start()
    {
        _keyCounter = FindObjectOfType<KeyCounter>();
        _chase = GetComponent<ChaseBehaviour>();
    }

    void Update()
    {
        if (_chase.dead == true && !_isDead)
        {
            _keyCounter.keysCollected++;
            _isDead = true;
        }
    }
}
