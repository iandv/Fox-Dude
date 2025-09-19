using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    AudioSource _audio;
    Animator _anim;
    GemCanvas _gemc;
    SpriteRenderer _sr;
    bool _feedBack;
    float _feedBackTime;


    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _gemc = FindObjectOfType<GemCanvas>();
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

            if (_feedBackTime >= 1)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            _audio.Play();
            GetComponent<BoxCollider2D>().enabled = false;
            _anim.SetBool("Feedback", true);
            _feedBack = true;
            _gemc.feedback = true;
        }
    }
}
