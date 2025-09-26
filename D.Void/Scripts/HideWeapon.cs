using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWeapon : MonoBehaviour
{
    public GameObject weapon;
    public PlayerSuper playerSuper;
    public bool isSuperGun;

    void Update()
    {
        if (!isSuperGun)
        {
            if (weapon.activeSelf && !playerSuper.skillActive)
                foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = true;
                }
            else
            {
                foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = false;
                }
            }
        }
        
        if (isSuperGun)
        {
            if (weapon.activeSelf && playerSuper.skillActive)
                foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = true;
                }
            else
            {
                foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = false;
                }
            }
        }
    }
}
