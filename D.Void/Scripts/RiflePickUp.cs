using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickUp : IPickUp
{
    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            var weaponHolder = FindObjectOfType<WeaponSwitching>();

            if (weaponHolder != null)
            {
                weaponHolder.hasRifle = true;
                pickUpCollider.enabled = false;
                HideMesh();
                pickUpAudioSource.PlayOneShot(pickUpSound);
                StartCoroutine(MyCoroutine());
            }
        }
    }

    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
