using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    protected ChaseBehaviour _myEnemy;
    [SerializeField]
    protected GameObject spawn;
    [SerializeField]
    protected ParticleSystem particleEffect;
    [SerializeField]
    protected float spawnTime;
    [SerializeField]
    protected AudioClip audioClip;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _myEnemy = GetComponentInParent<ChaseBehaviour>();
        if (_myEnemy != null)
        {
            StartCoroutine(SpawnCoroutine());
        }
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);
        if (!_myEnemy.dead)
        {
            _audioSource.Play();
            particleEffect.Play();
            Instantiate(spawn, transform.position, Quaternion.identity);
            StartCoroutine(SpawnCoroutine());
        }
    }
}
