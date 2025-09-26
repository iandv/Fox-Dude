using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewLevel : MonoBehaviour
{
    [SerializeField]
    protected float loadSec = 0.333f;
    [SerializeField]
    protected int levelIndex;
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

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Time.timeScale = 0f;
            loadText.text = loadingA;
            StartCoroutine(LoadTimeOne());
        }
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
        SceneManager.LoadScene(levelIndex);
    }
}
