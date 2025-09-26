using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : IPickUp
{
    [SerializeField]
    protected int ammo = 15;

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerAmmo = GameObject.FindGameObjectWithTag("Selected").GetComponent<PlayerGun>();

            if (playerAmmo != null && !playerAmmo.isMelee)
            {
                playerAmmo.GetComponent<PlayerGun>().ReceiveAmmo(ammo);
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
