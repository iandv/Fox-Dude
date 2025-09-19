using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : MonoBehaviour
{
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Animator _anim;
    AudioSource _audio;
    Player _player;
    public Bullet bulletPrefab;
    public int health;
    public bool stomp;
    public bool fire;
    public float rof;
    public float feedbackTime;
    public float flickeringTime;
    public float minRange;

    bool _death;
    float _deathTimer;
    float _currentROFTime;
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
        _player = FindObjectOfType<Player>();
        fire = false;
    }

    void Update()
    {
        Death();
        if (_death == true)
            return;
        Stomp();
        if (fire == true)
            Shoot();
        FeedbackDamage();
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

    void Shoot()
    {
        _currentROFTime += Time.deltaTime;
        if (_currentROFTime >= rof && Vector3.Distance(transform.position, _player.transform.position) >= minRange)
        {
            _currentROFTime = 0;
            Bullet bulletTemp = Instantiate(bulletPrefab);
            bulletTemp.transform.position = transform.position;
            bulletTemp.dir = _player.transform.position - transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
            TakeDamage();
    }
}
