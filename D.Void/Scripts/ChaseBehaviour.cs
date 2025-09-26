using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : EntityDeath
{
    private NavMeshAgent _navMesh;
    private GameObject _player;
    private Transform _playerTransform;
    private Animator _enemyAnimator;
    private EnemyEyes _enemyEyes;

    [SerializeField]
    public float distanceToTarget = 4f;
    [SerializeField]
    protected float lookAtPlayerDistance = 3f;
    [SerializeField]
    protected float rotationSpeed = 4f;
    [SerializeField]
    protected string velocityFloatName = "Velocity";

    void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player");
        _playerTransform = _player.transform;
        _enemyAnimator = GetComponent<Animator>();
        _enemyEyes = GetComponentInChildren<EnemyEyes>();
    }
    void FixedUpdate()
    {
        if (!dead && _player.GetComponent<PlayerAuxBool>().passive == false && !stun)
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);

            float velocity = _navMesh.velocity.magnitude / _navMesh.speed;

            _enemyAnimator.SetFloat(velocityFloatName, velocity);

            if (distance < distanceToTarget && _enemyEyes.playerSpotted == true)
            {
                Vector3 dirToPlayer = transform.position - _player.transform.position;
                Vector3 newPos = transform.position - dirToPlayer;
                _navMesh.SetDestination(newPos);
            }

            if (distance < lookAtPlayerDistance)
            {
                Vector3 relativePos = _playerTransform.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        if (dead)
        {
            _navMesh.enabled = false;
        }
    }
}
