using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gemsCollected;
    public int gemsLimit;
    public TextMeshProUGUI Counter;

    void Update()
    {
        Counter.text = "" + gemsCollected;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(gemsCollected >= gemsLimit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
