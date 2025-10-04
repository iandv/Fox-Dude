using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] float force = 1000f, radius = 10f, ExplosionEffectDuration = 2f, activationTime;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] int pointsValue;

    private Rigidbody _rb;
    private bool _active, _isHit, _moved;

    private void Awake()
    {
        _active = false;
        _isHit = false;
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

    void OnCollisionEnter(Collision collision)
    {
        if (_active)
        {
            float velocity = _rb.velocity.magnitude;
            if (velocity > 0.9f && !_isHit)
            {
                _isHit = true;
                Explosion();
            }
        }        
    }

    void Explosion()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            BurnObject bo = hit.GetComponent<BurnObject>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, explosionPosition, radius);
            }

            if (bo)
            {
                bo.Burning();
            }
        }
        EventManager.Instance.Trigger("points", pointsValue);
        GameManager.Instance.RemoveDestructibleFromList(gameObject);
        GameObject myEffect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(myEffect, ExplosionEffectDuration);
        Destroy(gameObject);
    }

    IEnumerator Activation()
    {
        yield return new WaitForSeconds(activationTime);
        _active = true;
    }
}
