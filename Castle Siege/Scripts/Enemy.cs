using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject destroyEffect;
    [SerializeField] int pointsValue;
    [SerializeField] float effectDuration, deathDuration;
    [SerializeField] Animator animator;
    [SerializeField] string fallingVelocityName = "Velocity", deathBoolName = "Dead";

    private bool _isHit, _moved;
    private Rigidbody _rb;
    private GameObject _parent;    

    void Awake()
    {
        _parent = transform.parent.gameObject;
        _rb = GetComponentInParent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.Instance.AddDesctructibleToList(_parent);
#if (UNITY_EDITOR)
        GameManager.Instance.ReceiveScore(pointsValue);
#endif
    }

    void Update()
    {
        float velocity = _rb.velocity.magnitude;
        if (velocity > 0.9f && !_moved && !_isHit)
        {
            _moved = true;
            GameManager.Instance.AddMovingItem(_parent);
        }

        if (velocity < 0.999f && _moved)
        {
            _moved = false;
            GameManager.Instance.RemoveMovingItem(_parent);
        }

        animator.SetFloat(fallingVelocityName, velocity);
    }

    void OnTriggerEnter(Collider collider)
    {
        float velocity = _rb.velocity.magnitude;
        if (velocity > 0.9f)
        {
            if ((collider.gameObject.layer == 0 || collider.gameObject.layer == 11) && !_isHit)
            {
                Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    if (!_isHit)
                    {
                        _isHit = true;
                        EventManager.Instance.Trigger("points", pointsValue);
                        GameManager.Instance.RemoveDestructibleFromList(_parent);
                    }

                    StartCoroutine(DestroyCoroutine());
                }
            }
        }
    }

    IEnumerator DestroyCoroutine()
    {
        animator.SetBool(deathBoolName, true);
        yield return new WaitForSeconds(deathDuration);
        GameObject myEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(myEffect, effectDuration);
        Destroy(_parent);
    }
}
