using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    protected float selfDestructTime = 2f, effectDuration = 2f;
    [SerializeField]
    protected GameObject destructEffect;

    protected Rigidbody _rb;

    virtual protected void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        EventManager.Instance.Trigger("bullet airborne");
        EventManager.Instance.Subscribe("destroy all bullets", DestroySelf);
    }

    protected void OnCollisionStay(Collision collision)
    {
        EventManager.Instance.Trigger("bullet grounded");

        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            float velocity = _rb.velocity.magnitude;
            if (velocity < 0.999f)
            {
                StartCoroutine(DestroyCoroutine());
            }
        }       
    }

    void DestroyEffect()
    {
        GameObject myEffect = Instantiate(destructEffect, transform.position, Quaternion.identity);
        Destroy(myEffect, effectDuration);
        EventManager.Instance.Trigger("can shoot");
        EventManager.Instance.Unsubscribe("destroy all bullets", DestroySelf);
        Destroy(gameObject);
    }

    public void DestroySelf(params object[] parameters)
    {
        DestroyEffect();
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(selfDestructTime);
        DestroyEffect();
    }

}
