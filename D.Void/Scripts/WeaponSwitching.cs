using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public int ammoInClip = 0;
    public int fullClip = 0;
    public float switchTime = 1f;
    public bool isSwitching = false;
    public GameObject selectedGun;
    public bool hasRifle;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        
        var gun = selectedGun.GetComponent<PlayerGun>();
        if (isSwitching == true || gun.isReloading == true)
            return;
        {
            if (hasRifle)
            {
                int previousSelectedWeapon = selectedWeapon;
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    if (selectedWeapon >= transform.childCount - 1)
                        selectedWeapon = 0;
                    else
                        selectedWeapon++;
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    if (selectedWeapon <= 0)
                        selectedWeapon = transform.childCount - 1;
                    else
                        selectedWeapon--;
                }

                if (previousSelectedWeapon != selectedWeapon)
                {
                    SelectWeapon();
                }
            }
            
            ammoInClip = selectedGun.GetComponent<PlayerGun>().ammoInClip;
            fullClip = selectedGun.GetComponent<PlayerGun>().ammoReserve;
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                isSwitching = true;
                weapon.gameObject.SetActive(true);
                weapon.gameObject.tag = "Selected";
                selectedGun = weapon.gameObject;
                selectedGun.GetComponent<PlayerGun>().WeaponSwitch();
                StartCoroutine(SwitchCoroutine());
            }

            else
            {
                weapon.gameObject.SetActive(false);
                weapon.gameObject.tag = "Untagged";
            }
            i++;
        }
    }

    IEnumerator SwitchCoroutine()
    {
        yield return new WaitForSeconds(switchTime);
        isSwitching = false;
    }
}
