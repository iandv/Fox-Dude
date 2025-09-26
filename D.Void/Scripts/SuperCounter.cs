using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCounter : MonoBehaviour
{
    PlayerAuxBool _playerBool;
    public int killsToSuper;
    public int killsMade;
    void Start()
    {
        _playerBool = FindObjectOfType<PlayerAuxBool>();
    }


    void Update()
    {
        if (killsMade > killsToSuper)
        {
            killsMade = killsToSuper;
        }

        if(!_playerBool.superIsCharged)
        {
            if (killsMade >= killsToSuper)
            {
                _playerBool.superIsCharged = true;
            }
        }
    }
}
