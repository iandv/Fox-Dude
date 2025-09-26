using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : EntityDeath
{
    [SerializeField]
    protected float _summonRange = 2.5f;
    [SerializeField]
    private GameObject _summonedSpawn;
    [SerializeField]
    protected float cooldownTime;
    [SerializeField]
    protected string summonTriggerName = "Summon";
    [SerializeField]
    protected AudioClip SummonRoar;

    private bool _enemySummon = true;
    private Animator _enemyAnimator;
    private AudioSource _enemyAudioSource;
    private GameObject _player;

    void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyAudioSource = GetComponent<AudioSource>();
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (!dead)
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);

            if (distance < _summonRange && _enemySummon == true)
            {
                _enemySummon = false;
                _enemyAnimator.SetTrigger(summonTriggerName);
                _enemyAudioSource.PlayOneShot(SummonRoar);

                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        _enemySummon = true;
    }
}
