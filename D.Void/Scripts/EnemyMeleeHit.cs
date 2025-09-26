using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHit : MonoBehaviour
{
    [SerializeField]
    protected int damage = 25;

    protected iDamage _auxDamage;

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _auxDamage = collision.gameObject.GetComponent<iDamage>();
            if (_auxDamage != null)
            {
                _auxDamage.ReceiveDamage(damage);
            }
        }
    }
}
