using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityDeath
{
    private CharacterController controller;
    private GameObject playerGun;
    private PlayerAuxBool playerAuxBool;
    private PlayerSuper playerSuper;
    public bool firedShot = false;

    [SerializeField]
    protected WeaponSwitching playerWeaponHolder;
    [SerializeField]
    protected GameObject debugConsole;
    [SerializeField]
    protected GameObject pauseMenu;

    private bool debugToggle = false;
    private bool isPaused = false;

    [SerializeField]
    protected float speed = 12f;

    [SerializeField]
    protected float gravity = -9.81f;

    [SerializeField]
    protected Transform groundCheck;

    [SerializeField]
    protected float groundDistance = 0.4f;

    [SerializeField]
    protected LayerMask groundMask;

    [SerializeField]
    protected float jumpHeight = 2f;

    [SerializeField]
    protected KeyCode debugKey = KeyCode.Tab;
    [SerializeField]
    protected KeyCode reloadKey = KeyCode.R;
    [SerializeField]
    protected KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField]
    protected KeyCode menuKey = KeyCode.Escape;
    [SerializeField]
    protected KeyCode interactKey = KeyCode.E;
    [SerializeField]
    protected KeyCode meleeKey = KeyCode.Mouse1;
    [SerializeField]
    protected KeyCode superKey = KeyCode.Q;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerGun = GameObject.FindGameObjectWithTag("Selected");
        playerAuxBool = GetComponent<PlayerAuxBool>();
        playerSuper = FindObjectOfType<PlayerSuper>();
    }

    private void Update()
    {

        if (!dead)
        {
            Console();
            PauseMenu();
            playerGun = GameObject.FindGameObjectWithTag("Selected");
            Movement();
            Interact();
            var gun = playerGun.GetComponent<PlayerGun>();
            if (gun.isReloading == true || playerWeaponHolder.isSwitching == true || isPaused == true || debugToggle == true)
                return;
            Fire();
            Melee();
            Reload();
            Super();
        }
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void Console()
    {
        if (Input.GetKeyDown(debugKey) && debugToggle == false)
        {
            debugToggle = !debugToggle;
            Cursor.lockState = CursorLockMode.None;
            debugConsole.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(debugKey) && debugToggle == true)
        {
            debugToggle = !debugToggle;
            Cursor.lockState = CursorLockMode.Locked;
            debugConsole.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void PauseMenu()
    {
        
        if (Input.GetKeyDown(menuKey) && isPaused == false)
        {
            isPaused = !isPaused;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(menuKey) && isPaused == true)
        {
            isPaused = !isPaused;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Reload()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            playerGun.GetComponent<PlayerGun>().Reload();
        }
    }

    void Fire()
    {
        var gun = playerGun.GetComponent<PlayerGun>();
        bool super = playerSuper.skillActive;

        if (gun != null && !super)
        {
            if (gun.fullAuto)
            {
                if (Input.GetKey(fireKey))
                {
                    if (gun.outOfAmmo)
                        gun.Reload();
                    else
                    {
                        gun.Shoot();
                        firedShot = true;
                        StartCoroutine(FireCoroutine());
                    }                        
                }
            }

            else
            {
                if (Input.GetKeyDown(fireKey))
                {
                    if (gun.outOfAmmo)
                        gun.Reload();
                    else
                    {
                        gun.Shoot();
                        firedShot = true;
                        StartCoroutine(FireCoroutine());
                    }                       
                }
            }
        }

        if (super)
        {
            if (Input.GetKeyDown(fireKey))
            {
                playerSuper.Shoot();
                firedShot = true;
                StartCoroutine(FireCoroutine());
            }         
        }
    }

    void Melee()
    {
        var gun = playerGun.GetComponent<PlayerGun>();

        if (Input.GetKeyDown(meleeKey))
        {
            gun.MeleeAttack();
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown(interactKey))
        {
            playerAuxBool.interact = true;
            StartCoroutine(UseCoroutine());

        }
    }

    void Super()
    {
        if (Input.GetKeyDown(superKey))
        {           
            if (playerAuxBool.superIsCharged)
            {
                playerSuper.ActivateSkill();
            }
        }
    }

    IEnumerator FireCoroutine()
    {
        yield return null;
        firedShot = false;
    }

    IEnumerator UseCoroutine()
    {
        yield return null;
        playerAuxBool.interact = false;
    }
}
