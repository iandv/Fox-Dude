using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasQuitGame : MonoBehaviour
{
    [SerializeField]
    private GameObject quitPopUp;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            quitPopUp.SetActive(true);
        }
    }
}
