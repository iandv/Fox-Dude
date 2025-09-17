using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    AudioSource _audio;
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Animator _anim;
    public Transform[] waypoints;
    int _nextWaypoint;
    public float minRange;
    public float speed;
    public float timeToFire;
    public int health;
    public int lapse;
    public int finalLapse;
    public float feedbackTime;
    public float flickeringTime;
    public float missileBurst;
    public Bullet bulletPrefab;
    public Missile missilePrefab;
    Player _player;

    float _currentFireTime;
    float _currentTime;
    float _originalTime;
    bool _isTakingDamage;
    bool _damageFeedback;
    bool _reset;
    bool _death;
    bool _specialAttack;
    float _deathTimer;
    float _missileTime;
    float _bCount;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _nextWaypoint = 0;
        transform.position = waypoints[0].position;
        _player = FindObjectOfType<Player>();
        _specialAttack = false;
    }

    void Update()
    {
        Death();
        if (_death == true)
            return;
        Move();
        FeedbackDamage();
        Shoot();
        Special();
        MissileAttack();
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[_nextWaypoint].position) <= minRange)
        {
            if (_nextWaypoint == waypoints.Length - 1)
            {
                lapse++;
                _reset = true;
            }

            else if (_nextWaypoint == 0)
                _reset = false;

            if (!_reset)
                _nextWaypoint++;
            else
                _nextWaypoint = 0;

            if ((waypoints[_nextWaypoint].position - transform.position).x < 0)
                _sr.flipX = false;
            else
                _sr.flipX = true;
        }
        transform.position += (waypoints[_nextWaypoint].position - transform.position).normalized * speed * Time.deltaTime;
    }

    void Shoot()
    {
        _currentFireTime += Time.deltaTime;
        if (_currentFireTime >= timeToFire && _specialAttack == false)
        {
            _currentFireTime = 0;
            Bullet bulletTemp = Instantiate(bulletPrefab);
            bulletTemp.transform.position = transform.position;
            bulletTemp.dir = _player.transform.position - transform.position;
        }
    }

    void Special()
    {
        if (lapse >= finalLapse)
        {
            _specialAttack = true;
            lapse = 0;
        }
    }

    void MissileAttack()
    {
        if (_specialAttack == true)
        {
            _missileTime += Time.deltaTime;
            if (_missileTime >= missileBurst)
            {
                _missileTime = 0;
                Missile missileTemp = Instantiate(missilePrefab);
                missileTemp.transform.position = transform.position;
                _bCount++;

                if (_bCount >= 3)
                {
                    _bCount = 0;
                    _specialAttack = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
            TakeDamage();
    }

    void TakeDamage()
    {
        if (_isTakingDamage)
            return;
        _audio.Play();
        health--;
        if (health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _anim.SetBool("Death", true);
            _death = true;
        }
        else
        {
            _sr.color = Color.red;
            _isTakingDamage = true;
            _damageFeedback = true;

        }
    }

    void Death()
    {
        if (_death == true)
        {
            _deathTimer += Time.deltaTime;
            if (_deathTimer >= 0.5f)
            {
                Destroy(gameObject);
                SceneManager.LoadScene(18);
            }
        }
    }

    void FeedbackDamage()
    {
        if (_isTakingDamage)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= feedbackTime)
            {
                if (_damageFeedback)
                {
                    _sr.color = Color.white;
                    _damageFeedback = false;
                }
                else
                {
                    _sr.color = Color.red;
                    _damageFeedback = true;
                }
                feedbackTime += _originalTime;
            }
            if (_currentTime >= flickeringTime)
            {
                _sr.color = Color.white;
                _currentTime = 0f;
                feedbackTime = _originalTime;
                _isTakingDamage = false;
            }
        }
    }



}
