using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{
    Player _player;
    SpriteRenderer _sr;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ResetGun();
        Fire();
        Flip();
    }

    public void Fire()
    {
        if (_player.canShoot == false)
        {
            _sr.enabled = true;
        }
            
    }

    public void ResetGun()
    {
        if (_player.canShoot == true)
        {
            _sr.enabled = false;
        }
    }

    public void Flip()
    {
        if (_player.GetComponent<SpriteRenderer>().flipX == true)
        {
            transform.position = _player.transform.position + new Vector3(-0.5f, -0.562f);
            _sr.flipX = true;
        }

        else
        {
            transform.position = _player.transform.position + new Vector3(0.5f, -0.562f);
            _sr.flipX = false;
        }
    }
}
