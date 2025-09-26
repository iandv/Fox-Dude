using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    protected EnemyHealth myHealth;
    protected iDamage _auxDamage;
    protected iStun _auxStun;
    protected int _hitDamage;


    void Awake()
    {
        myHealth = GetComponentInParent<EnemyHealth>();
    }
    public void IamHit(int hitDamage)
    {
        _auxDamage = myHealth;
        if (_auxDamage != null)
        {
            _auxDamage.ReceiveDamage(hitDamage);
        }
    }

    public void IamHitMelee(int hitMelee)
    {
        _auxStun = myHealth;
        if (_auxStun != null)
        {
            _auxStun.ReceiveMeleeStun(hitMelee);
        }
    }
}
