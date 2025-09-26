using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunUIWatcher : MonoBehaviour
{
    [SerializeField]
    protected WeaponSwitching watchedPlayerGun;
    [SerializeField]
    protected TextMeshProUGUI ammoCounter;
    [SerializeField]
    protected PlayerSuper playerSuper;

    protected void Update()
    {
        if (playerSuper.skillActive)
        {
            ammoCounter.text = playerSuper.ammoInClip + "/" + playerSuper.fullClip;
        }
        else
        {
            ammoCounter.text = watchedPlayerGun.ammoInClip + "/" + watchedPlayerGun.fullClip;
        }        
    }
}
