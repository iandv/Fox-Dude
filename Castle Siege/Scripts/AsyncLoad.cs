using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncLoad : MonoBehaviour
{
    public static AsyncLoad Instance { get; private set; }

    [HideInInspector]
    public bool isLoading;
    [SerializeField]
    private GameObject loaderUI;
    [SerializeField]
    private Slider slider;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        isLoading = false;
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

    private IEnumerator LoadSceneCoroutine(int index)
    {
        slider.value = 0;
        loaderUI.SetActive(true);
        isLoading = true;

        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;
        float progress = 0;
        while (!async.isDone)
        {
            progress = Mathf.MoveTowards(progress, async.progress, Time.deltaTime);
            slider.value = progress;
            if (progress >= 0.9f)
            {
                slider.value = 1;
                async.allowSceneActivation = true;
                Time.timeScale = 1;
                isLoading = false;
            }
            yield return null;
        }
    }
}
