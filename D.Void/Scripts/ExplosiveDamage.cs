using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDamage : MonoBehaviour
{
    public int damage = 500;
    public float damageWindow = 0.6f;

    protected bool _exploded;
    protected iDamage _auxDamage;
    protected void Awake()
    {
        StartCoroutine(DamageCoroutine());
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") && !_exploded)
        {
            _auxDamage = collision.gameObject.GetComponentInParent<iDamage>();
            if (_auxDamage != null)
            {
                _auxDamage.ReceiveDamage(damage);
            }
        }
    }

    IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(damageWindow);
        _exploded = true;
    }
}
