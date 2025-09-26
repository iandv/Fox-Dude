using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject[] items;
    private ChaseBehaviour _myEnemy;
    private bool _used;
    private void Start()
    {
        _myEnemy = GetComponentInParent<ChaseBehaviour>();
        _used = false;
    }

    private void Update()
    {
        if (_myEnemy.dead && !_used)
        {
            _used = !_used;
            int randomItem = UnityEngine.Random.Range(0, items.Length);
            Instantiate(items[randomItem], transform.position, Quaternion.identity);
        }
    }
}
