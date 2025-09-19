using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public float lifeSpan;
    float _timer;
    Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        LifeSpan();
        Follow();
    }

    public void Follow()
    {
        transform.up = _player.transform.position - transform.position;
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void LifeSpan()
    {
        _timer += Time.deltaTime;
        if (_timer > lifeSpan)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
