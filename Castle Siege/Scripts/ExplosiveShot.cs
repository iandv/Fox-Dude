using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShot : MonoBehaviour
{
    [SerializeField]
    private float force = 1000f, radius = 10f;

    private BulletBehaviour _bb;

    private void Start()
    {
        _bb = FindObjectOfType<BulletBehaviour>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    void Explosion()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, explosionPosition, radius);
            }
            
        }
        _bb.DestroySelf();
    }
}
