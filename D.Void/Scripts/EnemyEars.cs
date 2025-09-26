using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEars : MonoBehaviour
{
    private PlayerController _player;
    public EnemyEyes myOwnEyes;

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player = collision.GetComponent<PlayerController>();

            if (_player.firedShot)
            {
                myOwnEyes.playerSpotted = true;
            }
        }
    }
}
