using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPickUp : MonoBehaviour
{
    [SerializeField]
    protected AudioClip pickUpSound;

    protected AudioSource pickUpAudioSource;
    protected Collider pickUpCollider;
    protected MeshRenderer pickUpRenderer;

    private void Awake()
    {
        pickUpCollider = GetComponent<Collider>();
        pickUpAudioSource = GetComponent<AudioSource>();
        pickUpRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void HideMesh()
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }
    }
}
