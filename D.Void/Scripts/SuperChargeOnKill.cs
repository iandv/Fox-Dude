using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperChargeOnKill : MonoBehaviour
{
    protected SuperCounter _superCounter;
    protected ChaseBehaviour _chase;
    protected PlayerSuper _playerSuper;
    protected bool _isDead;
    void Start()
    {
        _superCounter = FindObjectOfType<SuperCounter>();
        _chase = GetComponent<ChaseBehaviour>();
        _playerSuper = FindObjectOfType<PlayerSuper>();
    }

    void Update()
    {
        if (_chase.dead == true && !_isDead && _playerSuper.skillActive)
        {
            _isDead = true;
        }

        if (_chase.dead == true && !_isDead && !_playerSuper.skillActive)
        {
            if (_superCounter.killsMade < _superCounter.killsToSuper)
            {
                _superCounter.killsMade++;
                _isDead = true;
            }        
        }          
    }
}
