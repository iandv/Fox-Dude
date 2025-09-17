using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerFeet>())
            transform.parent.GetComponent<Enemy>().stomp = true;
    }
}
