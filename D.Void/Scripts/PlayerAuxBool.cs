using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAuxBool : MonoBehaviour
{
    public bool passive;
    public bool godMode;
    public bool unlockedDoor;
    public bool interact;
    public bool superIsCharged;

    public void Awake()
    {
        passive = false;
        godMode = false;
        unlockedDoor = false;
        interact = false;
        superIsCharged = false;
    }
}
