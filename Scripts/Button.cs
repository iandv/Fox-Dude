using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject nextMenu;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeToTheNextLevelByNumber(int index)
    {
        Debug.Log(index);
        SceneManager.LoadScene(index);
    }

    public void ChangeToNextMenu()
    {
        currentMenu.SetActive(false);
        nextMenu.SetActive(true);
    }

    internal void Select()
    {
        throw new NotImplementedException();
    }
}
