using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnObject : MonoBehaviour
{
    public bool onFire;
    [SerializeField]
    private ParticleSystem[] fireParticles;
    [SerializeField]
    private GameObject destroyEffect;
    [SerializeField]
    private float effectDuration, maxBurnTime, burnDuration;
    [SerializeField]
    private int pointsValue;

    private float _time;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        GameManager.Instance.AddDesctructibleToList(gameObject);
#if (UNITY_EDITOR)
        GameManager.Instance.ReceiveScore(pointsValue);
#endif
    }

    private void OnCollisionEnter(Collision collision)
    {
        ExplosiveShot es = collision.gameObject.GetComponent<ExplosiveShot>();

        if (es != null)
        {
            Burning();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        BurnObject bo = collision.gameObject.GetComponent<BurnObject>();
        if (bo != null && bo.onFire)
        {
            _time += Time.deltaTime;
            if (_time >= maxBurnTime)
            {
                Burning();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        BurnObject bo = collision.gameObject.GetComponent<BurnObject>();
        if (bo != null && bo.onFire && !onFire)
        {
            _time = 0;
        }
    }

    public void Burning()
    {
        if (!onFire)
        {
            onFire = true;
            foreach (var item in fireParticles)
                item.Play();
            StartCoroutine(BurnCoroutine());
        }
    }

    IEnumerator BurnCoroutine()
    {
        yield return new WaitForSeconds(burnDuration);
        GameObject myEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(myEffect, effectDuration);
        EventManager.Instance.Trigger("points", pointsValue);
        GameManager.Instance.RemoveDestructibleFromList(gameObject);
        Destroy(gameObject);
    }
}
