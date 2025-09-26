using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    [SerializeField]
    protected GameObject currentMenu;
    [SerializeField]
    protected GameObject nextMenu;
    [SerializeField]
    protected PlayerController controller;
    [SerializeField]
    protected GameObject pauseMenu;
    [SerializeField]
    protected float loadSec = 0.333f;
    [SerializeField]
    protected TextMeshProUGUI loadText;
    [SerializeField]
    protected string loadingA = "Loading.";
    [SerializeField]
    protected string loadingB = "Loading..";
    [SerializeField]
    protected string loadingC = "Loading...";
    [SerializeField]
    protected bool nextLevelIsAMenu;

    protected int _levelIndex;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeToTheNextLevelByNumber(int index)
    {
        Time.timeScale = 1f;
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

    public void Unpause()
    {
        controller.Resume();
    }

    public void ChangeToNextLevelWithLoading(int index)
    {
        _levelIndex = index;
        Time.timeScale = 0f;
        loadText.text = loadingA;
        StartCoroutine(LoadTimeOne());
    }

    IEnumerator LoadTimeOne()
    {
        yield return new WaitForSecondsRealtime(loadSec);
        loadText.text = loadingB;
        StartCoroutine(LoadTimeTwo());
    }

    IEnumerator LoadTimeTwo()
    {
        yield return new WaitForSecondsRealtime(loadSec);
        loadText.text = loadingC;
        StartCoroutine(LoadTimeThree());
    }

    IEnumerator LoadTimeThree()
    {
        yield return new WaitForSecondsRealtime(loadSec);
        if (nextLevelIsAMenu)
            Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        SceneManager.LoadScene(_levelIndex);
    }
}
