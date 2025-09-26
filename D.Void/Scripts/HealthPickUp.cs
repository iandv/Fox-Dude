using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : IPickUp
{
    [SerializeField]
    protected int heal = 20;

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerHealth = collision.transform.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.maxedHealth == false)
            {
                playerHealth.ReceiveHealing(heal);
                pickUpCollider.enabled = false;
                pickUpRenderer.enabled = false;
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


    //Por si alguna vez me sirve
    /*private void OnTriggerEnter(Collider collision)
    {
        var playerHealth = collision.transform.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ReceiveHealing(health);
        }
            
    }*/


}
