using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemy : MonoBehaviour
{
    [SerializeField]
    protected GameObject deathEffect;
    [SerializeField]
    protected int damage = 34;
    [SerializeField]
    protected float explosionDuration = 2f;

    protected iDamage _auxDamage;
    private Transform _transform;
    private GameObject _myParent;

    private void Start()
    {
        _transform = gameObject.transform.parent;
        _myParent = _transform.gameObject;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject myExplosion = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(myExplosion, explosionDuration);
            Destroy(_myParent);
            _auxDamage = collision.gameObject.GetComponent<iDamage>();
            if (_auxDamage != null)
            {
                _auxDamage.ReceiveDamage(damage);
            }
        }
    }
}
