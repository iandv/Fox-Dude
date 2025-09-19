using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger2 : MonoBehaviour
{
    public EnemyThree enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
            enemy.fire = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
            enemy.fire = false;
    }
}
