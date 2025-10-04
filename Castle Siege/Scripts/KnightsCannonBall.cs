using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightsCannonBall : MonoBehaviour
{
    public Vector3 direction;
    [SerializeField]
    GameObject destroyEffect;
    [SerializeField]
    float effectDuration, speed, force, radius;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Railing Back")
        {
            StartCoroutine(DestroyCoroutine());
        }

        if (collision.gameObject.layer == 0 || collision.gameObject.layer == 8)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, radius);
                }

            }
        }
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(effectDuration);
        GameObject myEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(myEffect, effectDuration);
        Destroy(gameObject);
    }
}
