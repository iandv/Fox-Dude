using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : EntityDeath
{
    [SerializeField]
    protected string attackTriggerName = "Attack";
    [SerializeField]
    protected float attackRange = 2.5f;

    private Animator _enemyAnimator;
    private GameObject _player;
    private PlayerAuxBool _auxBool;

    public GameObject _triggerCollider;
    public static bool _triggerEnabled = false;

    void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
        _auxBool = _player.GetComponent<PlayerAuxBool>();
        _triggerCollider.SetActive (false);
    }
    void Update()
    {
        if (!dead)
        {
            if (!stun)
            {
                float distance = Vector3.Distance(transform.position, _player.transform.position);

                if (distance < attackRange && !_auxBool.passive)
                {
                    _enemyAnimator.SetTrigger(attackTriggerName);
                    _triggerCollider.SetActive(true);
                }

                else
                {
                    _triggerCollider.SetActive(false);
                }
            }            
        }

        if (dead || stun)
        {
            _triggerCollider.SetActive(false);
        }
    }
}
