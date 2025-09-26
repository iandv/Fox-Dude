using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEyes : EntityDeath
{
    public float range = 50;
    public bool playerSpotted;

    protected int _defaultLayerMask;
    protected int _colliderLayerMask;
    protected int _layerMask;


    void Start()
    {
        _defaultLayerMask = LayerMask.GetMask("Default");
        _colliderLayerMask = LayerMask.GetMask("Collider");
        _layerMask = _defaultLayerMask | _colliderLayerMask;
    }

    void Update()
    {
        if (!dead)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range * 2, _layerMask))
            {
                PlayerHealth player = hit.transform.parent.GetComponent<PlayerHealth>();
                if (player != null)
                    playerSpotted = true;

            }
        }
    }
}
