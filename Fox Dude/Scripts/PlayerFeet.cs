using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    AudioSource _audio;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HeadHitbox>())
        {
            _audio.Play();
            transform.parent.GetComponent<Player>().bounce = true;
        }

        else if (collision.gameObject.GetComponent<HeadHitbox2>())
        {
            _audio.Play();
            transform.parent.GetComponent<Player>().bounce = true;
        }

        else if (collision.gameObject.GetComponent<HeadHitbox3>())
        {
            _audio.Play();
            transform.parent.GetComponent<Player>().bounce = true;
        }
    }
}
