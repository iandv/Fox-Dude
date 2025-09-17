using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    AudioSource _audio;
    Animator _anim;
    Player _player;
    SpriteRenderer _sr;
    bool _feedBack;
    float _feedBackTime;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();
        _player = FindObjectOfType<Player>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Feedback();
    }

    void Feedback()
    {
        if (_feedBack == true)
        {
            _feedBackTime += Time.deltaTime;
            if (_feedBackTime >= 0.333f)
            {
                _sr.enabled = false;
            }

            if (_feedBackTime >= 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && _player.maxedHealth == false)
        {
            _audio.Play();
            GetComponent<BoxCollider2D>().enabled = false;
            _anim.SetBool("Feedback", true);
            _feedBack = true;
        }
    }
}

