using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public bool isActive = true;
    public bool fullAuto;
    public int ammoReserve = 30;
    public int fullClip = 30;

    public int ammoInClip { get; protected set; }
    [SerializeField]
    protected int damage = 30;
    [SerializeField]
    protected int meleeDamage = 10;
    [SerializeField]
    protected int stunDamage = 1;
    [SerializeField]
    protected float range = 100f;
    [SerializeField]
    protected float meleeRange = 1f;
    [SerializeField]
    protected AudioClip shootSound;
    [SerializeField]
    protected AudioClip meleeSound;
    [SerializeField]
    protected AudioClip reloadSound;
    [SerializeField]
    protected AudioClip hitSound;
    [SerializeField]
    protected float rateOfFire = 9f;
    [SerializeField]
    protected float meleeRateOfFire = 1f;
    [SerializeField]
    protected float reloadTime = 3f;
    [SerializeField]
    protected float swingTime = 0.3f;
    [SerializeField]
    protected string reloadTriggerName = "reload";
    [SerializeField]
    protected string firingTriggerName = "firing rifle";
    [SerializeField]
    protected string currentTypeOfGun = "rifle";
    [SerializeField]
    protected string TypeOfGunOne = "pistol";
    [SerializeField]
    protected string meleeTriggerName = "pistol melee";
    [SerializeField]
    protected Animator playerAnimator;
    [SerializeField]
    protected ParticleSystem muzzleFlash;
    [SerializeField]
    protected GameObject enemyImpact;
    [SerializeField]
    protected GameObject surfaceImpact;

    protected float nextTimeToFire = 0f;
    protected bool meleeHit;
    protected GameObject playerCamera;
    protected AudioSource playerAudioSource;
    protected int _layerMask;
    protected CameraRecoil _cameraRecoil;
    public bool outOfAmmo;
    public bool isSwitching = false;
    public bool isReloading = false;
    public bool isMelee = false;

    void Start()
    {
        ammoInClip = fullClip;
        outOfAmmo = false;
        _layerMask = LayerMask.GetMask("Default");
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponentInParent<Animator>();
        _cameraRecoil = FindObjectOfType<CameraRecoil>();
    }

    public void ReceiveAmmo(int ammoAmount)
    {
        ammoReserve += ammoAmount;
    }

    public void Shoot()
    {
        if (!outOfAmmo && Time.time >= nextTimeToFire && !meleeHit)
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
                HitBox enemy = hit.transform.GetComponent<HitBox>();
                if (enemy != null)
                {
                    GameObject myImpact = Instantiate(this.enemyImpact, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(myImpact, 1f);
                    enemy.IamHit(damage);
                }

                else
                {
                    GameObject faceImpact = Instantiate(this.surfaceImpact, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(faceImpact, 2f);
                }
            }
            if (ammoInClip == 0)
            {
                outOfAmmo = true;
            }
        }        
    }

    public void MeleeAttack()
    {
        if (Time.time >= nextTimeToFire)
        {
            meleeHit = true;
            nextTimeToFire = Time.time + 1f / meleeRateOfFire;
            playerAnimator.SetTrigger(meleeTriggerName);
            playerAudioSource.PlayOneShot(meleeSound);
            StartCoroutine(WhipCoroutine());
        }
    }

    public void Reload()
    {
        if (ammoInClip < fullClip && ammoReserve > 0)
        {
            if (ammoReserve >= fullClip)
            {
                int newAmmo = fullClip - ammoInClip;
                isReloading = true;
                playerAnimator.SetTrigger(reloadTriggerName);
                playerAudioSource.PlayOneShot(reloadSound);
                ammoInClip += newAmmo;
                ammoReserve -= newAmmo;
                outOfAmmo = false;
                StartCoroutine(ReloadCoroutine());
            }

            else
            {
                isReloading = true;
                playerAnimator.SetTrigger(reloadTriggerName);
                playerAudioSource.PlayOneShot(reloadSound);
                ammoInClip += ammoReserve;
                ammoReserve -= ammoReserve;
                outOfAmmo = false;
                StartCoroutine(ReloadCoroutine());
            }
            
        }
    }

    public void WeaponSwitch()
    {
        playerAnimator.SetBool(currentTypeOfGun, true);
        playerAnimator.SetBool(TypeOfGunOne, false);
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    IEnumerator WhipCoroutine()
    {
        yield return new WaitForSeconds(swingTime);
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, meleeRange, _layerMask))
        {
            HitBox enemy = hit.transform.GetComponent<HitBox>();
            if (enemy != null)
            {
                playerAudioSource.PlayOneShot(hitSound);
                GameObject myImpact = Instantiate(this.enemyImpact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(myImpact, 1f);
                enemy.IamHit(meleeDamage);
                enemy.IamHitMelee(stunDamage);
            }

            else
            {
                playerAudioSource.PlayOneShot(hitSound);
                GameObject faceImpact = Instantiate(this.surfaceImpact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(faceImpact, 2f);
            }
        }
        meleeHit = false;
    }
}
