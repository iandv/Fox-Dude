using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
     protected void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerBool = FindObjectOfType<PlayerAuxBool>();

            if (playerBool != null)
            {
                playerBool.unlockedDoor = true;
            }
        }
    }
}
