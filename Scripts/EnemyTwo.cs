using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Animator _anim;
    AudioSource _audio;
    Player _player;
    public Transform[] waypoints;
    public int health;
    public float minRange;
    public float speed;
    public float chaseRange;
    public bool stomp;
    public bool chase;
    public float feedbackTime;
    public float flickeringTime;

    int _nextWaypoint;
    bool _return;
    bool _death;
    float _deathTimer;
    float _currentTime;
    float _originalTime;
    bool _isTakingDamage;
    bool _damageFeedback;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _nextWaypoint = 0;
        transform.position = waypoints[0].position;
        chase = false;
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        Death();
        if (_death == true)
            return;
        Stomp();
        Chase();
        if (chase == true)
            return;
        Move();
        FeedbackDamage();
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[_nextWaypoint].position) <= minRange)
        {
            if (_nextWaypoint == waypoints.Length - 1)
                _return = true;

            else if (_nextWaypoint == 0)
                _return = false;

            if (!_return)
                _nextWaypoint++;
            else
                _nextWaypoint--;

            if ((waypoints[_nextWaypoint].position - transform.position).x < 0)
                _sr.flipX = false;
            else
                _sr.flipX = true;
        }
        transform.position += (waypoints[_nextWaypoint].position - transform.position).normalized * speed * Time.deltaTime;
    }

    void Stomp()
    {
        if (stomp == true)
        {
            TakeDamage();
            stomp = false;
        }
    }

    void TakeDamage()
    {

        _audio.Play();
        health--;
        if (health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
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

    void Death()
    {
        if (_death == true)
        {
            _deathTimer += Time.deltaTime;
            if (_deathTimer >= 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    void Chase()
    {
        if (chase == true)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) >= chaseRange)
            {
                transform.position += (_player.transform.position - transform.position).normalized * speed * Time.deltaTime;
            }

            if ((_player.transform.position - transform.position).x < 0)
                _sr.flipX = false;
            else
                _sr.flipX = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
            TakeDamage();
    }
}
