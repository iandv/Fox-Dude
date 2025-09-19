using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifeSpan;
    float _timer;
    public Vector3 dir;

    void Update()
    {
        LifeSpan();
        transform.position += dir.normalized * speed * Time.deltaTime;
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
