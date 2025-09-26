using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : EntityDeath
{
    [SerializeField]
    protected float range = 8f;
    [SerializeField]
    protected string attackTriggerName = "Attack";
    [SerializeField]
    protected int damage = 25;
    [SerializeField]
    protected AudioClip shootSound;
    [SerializeField]
    protected float rateOfFire = 3f;
    [SerializeField]
    protected GameObject rifleMuzzle;
    [SerializeField]
    protected ParticleSystem muzzleFlash;
    [SerializeField]
    protected GameObject playerImpact;
    [SerializeField]
    protected GameObject surfaceImpact;

    [Range(0.0f, 1.0f)]
    public float hitAccuracy = 0.5f;

    private Animator _enemyAnimator;
    private AudioSource _enemyAudioSouce;
    private GameObject _player;
    private float _nextTimeToFire = 0f;
    protected int _defaultLayerMask;
    protected int _colliderLayerMask;
    protected int _layerMask;
    protected PlayerAuxBool _auxBool;
    protected EnemyEyes _enemyEyes;


    protected iDamage _auxDamage;

    void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyAudioSouce = GetComponent<AudioSource>();
        _defaultLayerMask = LayerMask.GetMask("Default");
        _colliderLayerMask = LayerMask.GetMask("Collider");
        _layerMask = _defaultLayerMask | _colliderLayerMask;
        _player = GameObject.FindWithTag("Player");
        _auxBool = _player.GetComponent<PlayerAuxBool>();
        _enemyEyes = GetComponentInChildren<EnemyEyes>();
    }


    void Update()
    {
        if (!dead)
        {
            if (!stun)
            {
                float distance = Vector3.Distance(transform.position, _player.transform.position);

                if (distance < range && Time.time >= _nextTimeToFire && !_auxBool.passive && _enemyEyes.playerSpotted == true)
                {
                    muzzleFlash.Play();
                    _enemyAnimator.SetTrigger(attackTriggerName);
                    _enemyAudioSouce.PlayOneShot(shootSound);
                    _nextTimeToFire = Time.time + 1f / rateOfFire;
                    RaycastHit hit;

                    if (Physics.Raycast(rifleMuzzle.transform.position, rifleMuzzle.transform.forward, out hit, range * 2, _defaultLayerMask))
                    {
                        GameObject faceImpact = Instantiate(this.surfaceImpact, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(faceImpact, 2f);
                    }

                    if (Physics.Raycast(rifleMuzzle.transform.position, rifleMuzzle.transform.forward, out hit, range * 2, _layerMask))
                    {
                        PlayerHealth player = hit.transform.parent.GetComponent<PlayerHealth>();
                        if (player != null)
                        {
                            float random = Random.Range(0.0f, 1.0f);
                            bool isHit = random > 1.0f - hitAccuracy;
                            if (isHit)
                            {
                                _auxDamage = _player.GetComponent<iDamage>();
                                if (_auxDamage != null)
                                {
                                    GameObject myImpact = Instantiate(this.playerImpact, hit.point, Quaternion.LookRotation(hit.normal));
                                    Destroy(myImpact, 1f);
                                    _auxDamage.ReceiveDamage(damage);
                                }
                            }

                            else if (Physics.Raycast(rifleMuzzle.transform.position, rifleMuzzle.transform.forward, out hit, range * 2, _defaultLayerMask))
                            {
                                GameObject faceImpact = Instantiate(this.surfaceImpact, hit.point, Quaternion.LookRotation(hit.normal));
                                Destroy(faceImpact, 2f);
                            }
                        }
                    }
                }
            }            
        }
    }
}
