using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform [] waypoints;
    int _nextWaypoint;
    public float minRange;
    public float speed;
    bool _return;

    void Start()
    {
        _nextWaypoint = 0;
        transform.position = waypoints[0].position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[_nextWaypoint].position) <= minRange)
        {
            if (_nextWaypoint == waypoints.Length - 1)
                _return = true;
            else if (_nextWaypoint == 0)
                _return = false;
            if (!_return)
                _nextWaypoint++;
            else
                _nextWaypoint--;
        }
        transform.position += (waypoints[_nextWaypoint].position - transform.position).normalized * speed * Time.deltaTime;
    }
}
