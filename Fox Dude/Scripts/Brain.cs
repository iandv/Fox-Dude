using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Brain
{
    public Player player;

    public void ListenKeys()
    {
        player.Move(Input.GetAxis("Horizontal"));
        if (Input.GetKeyDown(KeyCode.Space))
            player.Jump();
        if (Input.GetKeyDown(KeyCode.J))
            player.Shoot();
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            player.GoToNextLevel();
        if (Input.GetKeyDown(KeyCode.Keypad4))
            player.GoToPreviousLevel();
    }
}
