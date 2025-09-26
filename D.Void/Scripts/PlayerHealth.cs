using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, iDamage
{
    [SerializeField]
    protected int baseHealth = 100;
    [SerializeField]
    protected string deathBoolName = "dead";
    [SerializeField]
    protected AudioClip hurtSound;
    [SerializeField]
    protected AudioClip deathSound;
    [SerializeField]
    protected GameObject deathScreen;

    protected int currentHealth;
    protected PlayerMouseLook playerMouseLook;
    protected PlayerController playerController;
    protected CapsuleCollider playerCollider;
    protected Animator playerAnimator;
    protected AudioSource playerAudioSource;
    protected PlayerAuxBool playerAuxBool;

    public bool maxedHealth;

    private void Awake()
    {
        currentHealth = baseHealth;
        if (currentHealth >= baseHealth)
            maxedHealth = true;
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        playerMouseLook = GetComponentInChildren<PlayerMouseLook>();
        playerController = GetComponent<PlayerController>();
        playerCollider = GetComponentInChildren<CapsuleCollider>();
        playerAuxBool = GetComponent<PlayerAuxBool>();
    }

    public void ReceiveHealing(int healAmount)
    {
        if (currentHealth < baseHealth && healAmount > 0)
        {
            currentHealth += healAmount;
            if (currentHealth >= baseHealth)
            {
                maxedHealth = true;
                currentHealth = baseHealth;
            }
        }
    }
    public void ReceiveDamage(int damageAmount)
    {
        if (currentHealth > 0 && damageAmount > 0 && playerAuxBool.godMode == false)
        {
            currentHealth -= damageAmount;

            if (currentHealth < baseHealth)
            {
                maxedHealth = false;
            }

            if (currentHealth <= 0)
            {
                playerAudioSource.PlayOneShot(deathSound);
                playerAnimator.SetBool(deathBoolName, true);
                deathScreen.SetActive(true);
                playerCollider.enabled = false;
                playerMouseLook.dead = true;
                playerController.dead = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                playerAudioSource.PlayOneShot(hurtSound);
            }
        }
    }

    public string HealthToString
    {
        get
        {
            return "" + currentHealth;
        }
    }
}
