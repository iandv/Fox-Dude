using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEmpty : MonoBehaviour
{
    AudioSource _audio;
    public AudioClip spikesUp;
    public AudioClip spikesDown;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            _audio.Stop();
            _audio.clip = spikesUp;
            _audio.Play();
            transform.parent.GetComponent<Trap>().trapOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var playerTemp = collision.gameObject.GetComponent<Player>();
        if (playerTemp != null)
        {
            _audio.Stop();
            _audio.clip = spikesDown;
            _audio.Play();
            transform.parent.GetComponent<Trap>().trapOn = false;
        }
    }
}
