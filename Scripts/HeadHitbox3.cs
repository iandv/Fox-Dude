using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitbox3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerFeet>())
            transform.parent.GetComponent<EnemyThree>().stomp = true;
    }
}
