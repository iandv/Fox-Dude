using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Transform[] waypoints;
    public bool trapOn;
    public float speed;

    bool _reachedTop;

    void Start()
    {
        _reachedTop = false;
        trapOn = false;
    }

    void Update()
    {
        TrapBehavior();
    }

    void TrapBehavior()
    {
        if (trapOn == true && _reachedTop == false)
        {
            transform.position += (waypoints[1].position - transform.position).normalized * speed * Time.deltaTime;

            if ((waypoints[1].position - transform.position).y < 0)
                _reachedTop = true;

        }

        if (trapOn == false)
        {
            transform.position += (waypoints[0].position - transform.position).normalized * speed * Time.deltaTime;

            if ((waypoints[0].position - transform.position).y > 0)
                _reachedTop = false;
        }
    }
}
