using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, iDamage, iStun
{
    public int health = 100;
    [SerializeField]
    protected string deathBoolName = "dead";
    [SerializeField]
    protected string stunTriggerName = "stun";
    [SerializeField]
    protected AudioClip deathSound;
    [SerializeField]
    protected AudioClip stunSound;
    [SerializeField]
    protected float stunTime = 2f;
    [SerializeField]
    protected int stunHit = 1;
    [SerializeField]
    protected bool stunable;

    protected Animator myAnimator;
    protected AudioSource myAudioSource;
    protected MeleeAttack myMeleeAttack;
    protected RangedAttack myRangedAttack;
    protected ChaseBehaviour myChaseBehaviour;
    protected EnemyEyes myEyes;
    protected bool stun;

    void Awake()
    {
        myAnimator = GetComponentInChildren<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        myMeleeAttack = GetComponent<MeleeAttack>();
        myRangedAttack = GetComponent<RangedAttack>();
        myChaseBehaviour = GetComponent<ChaseBehaviour>();
        myEyes = GetComponentInChildren<EnemyEyes>();
    }
    public void ReceiveDamage(int damageAmount)
    {
        health -= damageAmount;
        myChaseBehaviour.distanceToTarget = 10000f;
        myEyes.playerSpotted = true;
        if (health <= 0f)
        {
            Death();
        }
    }

    public void ReceiveMeleeStun(int meleeStun)
    {
        if (stunable)
        {
            if (!stun)
            {
                stunHit += meleeStun;
                if (stunHit >= 2)
                {
                    Stun();
                }
            }
        }       
    }

    void Death()
    {
        if (myMeleeAttack != null)
        {
            myMeleeAttack.dead = true;
        }

        if (myRangedAttack != null)
        {
            myRangedAttack.dead = true;
        }

        myAnimator.SetBool(deathBoolName, true);
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("UpperBody"), 0f);
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("Stunlayer"), 0f);
        myAudioSource.PlayOneShot(deathSound);
        myChaseBehaviour.dead = true;
        myEyes.dead = true;
        stun = false;

        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }

    public void Stun()
    {
        if (myMeleeAttack != null)
        {
            myMeleeAttack.stun = true;
        }

        if (myRangedAttack != null)
        {
            myRangedAttack.stun = true;
        }

        myAnimator.SetTrigger(stunTriggerName);

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("Stunlayer"), 1f);
        myAudioSource.PlayOneShot(stunSound);
        myChaseBehaviour.stun = true;
        stun = true;
        StartCoroutine(StunCoroutine());
    }

    IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(stunTime);

        if (myMeleeAttack != null)
        {  
            myMeleeAttack.stun = false;
        }

        if (myRangedAttack != null)
        {
            myRangedAttack.stun = false;
        }

        myChaseBehaviour.stun = false;

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("Stunlayer"), 0f);
        stun = false;
        stunHit = 0;
    }
}
