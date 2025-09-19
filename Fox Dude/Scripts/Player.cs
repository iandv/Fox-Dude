using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Brain _brain;
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Animator _anim;
    GameManager _manager;
    AudioSource _audio;
    public AudioClip jump;
    public AudioClip hurt;
    public AudioClip death;
    public AudioClip water;
    public Bullet bulletPrefab;
    public PlayerFeet hitbox;
    public float speed;
    public float jumpForce;
    public float flickeringTime;
    public float feedbackTime;
    public float invFeedbackTime;
    public float maxHealth;
    public float rateOfFire;
    public float bulletVectorY;
    public float poisonCap;
    public Image healthBar;
    public Image gunImage;
    public SpriteRenderer invulnerable;
    public SpriteRenderer inverted;
    public Transform childCamera;
    public GameObject restartButton;
    public GameObject backButton;
    public bool gun;
    public bool bounce;
    public bool maxedHealth;
    public bool canShoot;
    public int endOfGame;
    public int startOfGame;

    float _health;
    float _deathForce;
    float _currentTime;
    float _originalTime;
    float _gameOver;
    float _jumpForce;
    float _timer;
    float _origSpeed;
    float _waterSpeed;
    float _poisonedTime;
    bool _isJumping;
    bool _isTakingDamage;
    bool _damageFeedback;
    bool _invulnerable;
    bool _invFeedback;
    bool _deathScene;
    bool _poisoned;

    void Start()
    {
        _waterSpeed = speed / 2;
        _origSpeed = speed;
        _audio = GetComponent<AudioSource>();
        _jumpForce = 12;
        _deathForce = 8;
        _timer = 0;
        _originalTime = flickeringTime;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _brain = new Brain();
        _brain.player = this;
        _health = maxHealth;
        canShoot = true;
        _manager = FindObjectOfType<GameManager>();
        maxedHealth = true;
    }

    void Update()
    {
        _brain.ListenKeys();
        CheckFall();
        FeedbackDamage();
        FeedbackInv();
        DeathScene();
        Bounce();
        if (_poisoned == true)
            Poisoned();
        if (gun == true)
            GunUI();
        if (canShoot == false)
            RateOfFire();
    }

    public void Poisoned()
    {
        inverted.enabled = true;
        _poisonedTime += Time.deltaTime;
        {
            if (_poisonedTime >= poisonCap)
            {
                _poisoned = false;
                inverted.enabled = false;
                _poisonedTime = 0;
            }
        }
    }

    public void RateOfFire()
    {
        _timer += Time.deltaTime;
        {
            if (_timer >= rateOfFire)
            {
                canShoot = true;
                _timer = 0;
            }
        }
    }

    public void Bounce()
    {
        if (_rb.velocity.y >= 0)
            bounce = false;

        if (bounce == true)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Shoot()
    {
        if (gun == true && canShoot == true)
        {
            canShoot = false;

            Bullet bulletTemp = Instantiate(bulletPrefab);
            bulletTemp.transform.position = transform.position + new Vector3(0, bulletVectorY);
            if (_sr.flipX)
                bulletTemp.dir = -transform.right;
            else
                bulletTemp.dir = transform.right;
        }
        
    }

    public void Move(float dir)
    {
        _anim.SetFloat("Speed", Mathf.Abs(dir));

        if (_poisoned == false)
        {
            _rb.velocity = new Vector2(dir * speed, _rb.velocity.y);
            if (dir > 0)
                _sr.flipX = false;
            else if (dir < 0)
                _sr.flipX = true;
        }

        if (_poisoned == true)
        {
            _rb.velocity = new Vector2(dir * speed *-1, _rb.velocity.y);
            if (dir < 0)
                _sr.flipX = false;
            else if (dir > 0)
                _sr.flipX = true;
        }

    }

    public void Jump()
    {
        if (_isJumping)
            return;
        _audio.Stop();
        _audio.clip = jump;
        _audio.Play();
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _isJumping = true;
        _anim.SetBool("IsJumping", _isJumping);
    }

    void CheckFall()
    {
        if (_rb.velocity.y < -1 && !_isJumping)
        {
            _anim.SetBool("IsFalling", true);
            _anim.SetBool("IsJumping", false);
        }
    }

    public void TakeDamage()
    {
        if (_isTakingDamage || _invulnerable)
            return;
        _audio.Stop();
        _audio.clip = hurt;
        _audio.Play();
        _health--;
        healthBar.fillAmount = _health / maxHealth;
        maxedHealth = false;
        if (_health <= 0)
            Death();
        else
        {
            _sr.color = Color.red;
            _isTakingDamage = true;
            _damageFeedback = true;
            
        }
    }

    void Invulnerable()
    {
        _sr.color = Color.yellow;
        _invulnerable = true;
        _invFeedback = true;
        invulnerable.enabled = true;
    }

    void GunUI()
    {
        gunImage.enabled = true;
    }

    void Death()
    {
        _audio.Stop();
        _audio.clip = death;
        _audio.Play();
        _rb.AddForce(Vector2.up * _deathForce, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        _anim.SetBool("Death", true);
        Destroy(hitbox);
        childCamera.parent = null;
        _deathScene = true;
    }

    void DeathScene()
    {
        if (_deathScene == true)
        {
            restartButton.SetActive(true);
            backButton.SetActive(true);
        }
    }

    void FeedbackDamage()
    {
        if (_isTakingDamage)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= flickeringTime)
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
                flickeringTime += _originalTime;
            }
            if (_currentTime >= feedbackTime)
            {
                _sr.color = Color.white;
                _currentTime = 0f;
                flickeringTime = _originalTime;
                _isTakingDamage = false;
            }
        }
    }

    void FeedbackInv()
    {
        if (_invulnerable)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= flickeringTime)
            {
                if (_invFeedback)
                {
                    _sr.color = Color.white;
                    _invFeedback = false;
                }
                else
                {
                    _sr.color = Color.yellow;
                    _invFeedback = true;
                }
                flickeringTime += _originalTime;
            }
            if (_currentTime >= invFeedbackTime)
            {
                _sr.color = Color.white;
                _currentTime = 0f;
                flickeringTime = _originalTime;
                _invulnerable = false;
                invulnerable.enabled = false;
            }
        }
    }

    public void GoToNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex != endOfGame)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToPreviousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex != startOfGame)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var platformTemp = collision.gameObject.GetComponent<Platform>();
        if (platformTemp)
            transform.parent = collision.transform;

        if (collision.gameObject.layer == 8)
        {
            _isJumping = false;
            _anim.SetBool("IsJumping", _isJumping);
            _anim.SetBool("IsFalling", false);
        }

        if (collision.gameObject.GetComponent<Enemy>() && bounce == false || (collision.gameObject.GetComponent<EnemyThree>() && bounce == false))
            TakeDamage();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyTwo>() && bounce == false)
            TakeDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 19 && _invulnerable == false)
            _poisoned = true;

        if (collision.gameObject.layer == 18)
        {
            _audio.Stop();
            _audio.clip = water;
            _audio.Play();
            speed = _waterSpeed;
        }

        if (collision.gameObject.GetComponent<Boss>() || (collision.gameObject.GetComponent<Trap>()) || 
            (collision.gameObject.GetComponent<Bullet>()) || (collision.gameObject.GetComponent<Missile>()))
            TakeDamage();

        if (collision.gameObject.GetComponent<Trap>())
            TakeDamage();

        if (collision.gameObject.GetComponent<Bullet>())
            TakeDamage();

        if (collision.gameObject.GetComponent<PickUp>())
            _manager.gemsCollected += 1;

        if (collision.gameObject.GetComponent<Invulnerability>())
            Invulnerable();

        if (collision.gameObject.GetComponent<Gun>())
            gun = true;

        if (collision.gameObject.GetComponent<Healing>() && _health < maxHealth)
        {
            _health++;
            if (_health >= maxHealth)
                maxedHealth = true;
            healthBar.fillAmount = _health / maxHealth;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var waterTemp = collision.gameObject.layer == 18;
        if (waterTemp != null)
        {
            speed = _origSpeed;
        }

        var platformTemp = collision.gameObject.GetComponent<Platform>();
        if (platformTemp != null)
        {
            transform.parent = null;
        }
    }
}
