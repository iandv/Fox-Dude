using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Animator _anim;
    AudioSource _audio;
    public GameObject gemPrefab;
    public Transform[] waypoints;
    public int health;
    public float minRange;
    public float speed;
    public bool stomp;
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
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _nextWaypoint = 0;
        transform.position = waypoints[0].position;
    }

    void Update()
    {
        Death();
        if (_death == true)
            return;
        Move();
        Stomp();
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
            //this is done to call the children and get its components.
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
                GameObject gemTemp = Instantiate(gemPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
            TakeDamage();
    }
}
