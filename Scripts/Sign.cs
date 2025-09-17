using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sign : MonoBehaviour
{
    TextMeshPro _tmp;

    void Start()
    {
        _tmp = GetComponent<TextMeshPro>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
            _tmp.enabled = true;
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
            _tmp.enabled = false;

    }
}
