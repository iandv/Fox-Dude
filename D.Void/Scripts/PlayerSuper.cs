using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuper : MonoBehaviour
{
    public int ammoReserve = 0;
    public int fullClip = 6;

    public int ammoInClip { get; protected set; }
    [SerializeField]
    protected int damage = 150;
    [SerializeField]
    protected float range = 100f;
    [SerializeField]
    protected float rateOfFire = 7f;
    [SerializeField]
    protected string firingTriggerName = "firing pistol";
    [SerializeField]
    protected AudioClip shootSound;
    [SerializeField]
    protected Animator playerAnimator;
    [SerializeField]
    protected ParticleSystem muzzleFlash;
    [SerializeField]
    protected GameObject surfaceImpact;
    [SerializeField]
    protected float explosionDuration = 2f;
    [SerializeField]
    protected string currentTypeOfGun = "pistol";
    [SerializeField]
    protected string TypeOfGunOne = "rifle";

    protected float nextTimeToFire = 0f;
    protected GameObject playerCamera;
    protected AudioSource playerAudioSource;
    protected int _layerMask;
    protected CameraRecoil _cameraRecoil;
    public bool outOfAmmo;
    public bool skillActive;

    private PlayerAuxBool _playerBool;
    private SuperCounter _superCounter;

    void Start()
    {
        ammoInClip = fullClip;
        outOfAmmo = false;
        _layerMask = LayerMask.GetMask("Default");
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponentInParent<Animator>();
        _cameraRecoil = FindObjectOfType<CameraRecoil>();
        _playerBool = FindObjectOfType<PlayerAuxBool>();
        _superCounter = FindObjectOfType<SuperCounter>();
    }

    public void Shoot()
    {
        if (!outOfAmmo && Time.time >= nextTimeToFire)
        {
            muzzleFlash.Play();
            nextTimeToFire = Time.time + 1f / rateOfFire;
            ammoInClip--;
            playerAnimator.SetTrigger(firingTriggerName);
            playerAudioSource.PlayOneShot(shootSound);
            _cameraRecoil.Recoil();
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range, _layerMask))
            {
                GameObject faceImpact = Instantiate(this.surfaceImpact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(faceImpact, explosionDuration);
            }
            if (ammoInClip == 0)
            {
                outOfAmmo = true;
                DeactivateSkill();
            }
        }
    }

    public void ActivateSkill()
    {
        _superCounter.killsMade = 0;
        skillActive = true;
        outOfAmmo = false;
        ammoInClip = fullClip;
        playerAnimator.SetBool(currentTypeOfGun, true);
        playerAnimator.SetBool(TypeOfGunOne, false);
    }

    public void DeactivateSkill()
    {
        _superCounter.killsMade = 0;
        _playerBool.superIsCharged = false;
        GameObject currentWeapon = GameObject.FindGameObjectWithTag("Selected");
        currentWeapon.GetComponent<PlayerGun>().WeaponSwitch();

        StartCoroutine(WaitFrame());
    }

    IEnumerator WaitFrame()
    {
        yield return null;
        skillActive = false;
    }
}
