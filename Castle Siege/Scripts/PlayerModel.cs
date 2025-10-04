using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour
{
    public float maxForce = 1000f, maxForcePressTime = 2f, 
        rotationSpeed, minRotation, maxRotation;

    [Range(10, 100)]
    public int linePoints;
    [Range(0.01f, 0.25f)]
    public float timeBetweenPoints;

    public Transform muzzle;
    public ParticleSystem muzzleFlash;
    public AudioClip cannonFireSound, reloadSound;
    public Image powerBar;

    public LineRenderer lineRenderer;

    public enum AmmoType
    {
        basic,
        triple,
        explosive
    }

    public AmmoType selectedAmmo;
    public int[] ammoStowage;
    public GameObject[] ammoPrefabs;
}
