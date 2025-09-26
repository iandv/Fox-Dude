using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    protected GameObject _player;

    static public bool god = false;
    static public bool openSesame = false;
    static public bool passive = false;

    [SerializeField]
    protected int heal = 25;
    [SerializeField]
    protected int ammo = 15;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    public void ProcessInput()
    {
        if (inputField.text.Equals("godmode"))
        {
            god = !god;
            Debug.Log("godmode " + god);
            var playerGodMode = _player.GetComponent<PlayerAuxBool>();

            if (god == true)
            {
                playerGodMode.godMode = true;
            }
            else if (god == false)
            {
                playerGodMode.godMode = false;
            }
        }

        if (inputField.text.Equals("health"))
        {
            Debug.Log("HP restored");
            var playerHealth = _player.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.maxedHealth == false)
            {
                playerHealth.ReceiveHealing(heal);
            }
        }

        if (inputField.text.Equals("lead"))
        {
            Debug.Log("Ammo restored");
            foreach (PlayerGun playerAmmo in FindObjectsOfType<PlayerGun>())

                if (playerAmmo != null)
                {
                    playerAmmo.ReceiveAmmo(ammo);
                }

        }

        if (inputField.text.Equals("opensesame"))
        {
            openSesame = !openSesame;
            var playerAuxBool = _player.GetComponent<PlayerAuxBool>();

            if (playerAuxBool != null && playerAuxBool.unlockedDoor == false)
            {
                Debug.Log("All doors unlocked " + openSesame);
                playerAuxBool.unlockedDoor = true;
            }

            else
            {
                Debug.Log("All doors locked " + openSesame);
                playerAuxBool.unlockedDoor = false;
            }
        }

        if (inputField.text.Equals("passive"))
        {
            passive = !passive;
            Debug.Log("passive " + passive);
            var playerPassive = _player.GetComponent<PlayerAuxBool>();

            if (passive == true)
            {
                playerPassive.passive = true;
            }
            else if (passive == false)
            {
                playerPassive.passive = false;
            }
        }

        inputField.text = string.Empty;
    }
}
