using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPointUp, spawnPointDown;
    [SerializeField]
    private GameObject munition, smokeEffect;
    [SerializeField]
    private float splitEffectDuration = 2f, tripleShotTime = 2f;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(tripleShotTime);
        GameObject myEffect = Instantiate(smokeEffect, transform.position, Quaternion.identity);
        GameObject ballOne = Instantiate(munition, spawnPointUp.position, spawnPointUp.rotation);
        ballOne.GetComponent<Rigidbody>().velocity = _rb.velocity - new Vector3(0, _rb.velocity.y / 2, 0);
        GameObject ballTwo = Instantiate(munition, spawnPointDown.position, spawnPointDown.rotation);
        ballTwo.GetComponent<Rigidbody>().velocity = _rb.velocity + new Vector3(0, _rb.velocity.y / 2, 0);
        Destroy(myEffect, splitEffectDuration);
    }
}
