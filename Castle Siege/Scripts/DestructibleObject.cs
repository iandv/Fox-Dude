using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyEffect;
    [SerializeField]
    private float effectDuration, activationTime, noDamageTime = 0.25f;
    [SerializeField]
    private int health, pointsValue;

    private bool _active, _isHit, _moved;
    private Rigidbody _rb;

    private void Awake()
    {
        _isHit = false;
        _active = false;
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(Activation());
    }

    private void Start()
    {
        GameManager.Instance.AddDesctructibleToList(gameObject);
#if (UNITY_EDITOR)
        GameManager.Instance.ReceiveScore(pointsValue);
#endif
    }

    private void Update()
    {
        float velocity = _rb.velocity.magnitude;
        if (velocity > 0.9f && !_moved && !_isHit)
        {
            _moved = true;
            GameManager.Instance.AddMovingItem(gameObject);
        }

        if (velocity < 0.999f && _moved)
        {
            _moved = false;
            GameManager.Instance.RemoveMovingItem(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_active)
        {
            float velocity = _rb.velocity.magnitude;
            float cVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            if ((velocity > 0.9f) || (cVelocity > 0.9f))
            {
                if (collision.gameObject && !_isHit)
                {
                    Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        _isHit = true;
                        health--;
                        StartCoroutine(Invulnerable());

                        if (health <= 0 && _isHit)
                        {
                            
                            GameObject myEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
                            Destroy(myEffect, effectDuration);
                            EventManager.Instance.Trigger("points", pointsValue);
                            GameManager.Instance.RemoveDestructibleFromList(gameObject);
                            Destroy(gameObject);
                        }                           
                    }
                }
            }
        }      
    }

    IEnumerator Invulnerable()
    {
        yield return new WaitForSeconds(noDamageTime);
        _isHit = false;
    }
    IEnumerator Activation()
    {
        yield return new WaitForSeconds(activationTime);
        _active = true;
    }
}
