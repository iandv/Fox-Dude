using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] GameObject currentMenu, nextMenu;
    bool _isPaused;

    public void PlayTouchSound()
    {
        if (FeedbackManager.Instance != null)
        {
            FeedbackManager.Instance.PlayTouchSoundOnce();
        }
    }

    public void ChangeMenu()
    {
        nextMenu.SetActive(true);
        currentMenu.SetActive(false);
    }

    public void LoadLevel(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void LoadAsync(int index)
    {
        Time.timeScale = 1f;
        AsyncLoad.Instance.LoadLevel(index);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }

    public void AsyncRestart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        AsyncLoad.Instance.LoadLevel(scene);
    }

    public void PauseMenu()
    {
        if (_isPaused == false)
        {
            nextMenu.SetActive(true);
            _isPaused = !_isPaused;
            Time.timeScale = 0f;
        }

        else if (_isPaused == true)
        {
            nextMenu.SetActive(false);
            _isPaused = !_isPaused;
            Time.timeScale = 1f;
        }
    }

    public void ResetSave()
    {
        PlayerSaveProfile.Instance.ResetSave();
        MainMenuUIWatcher.Instance.UpdateUI();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClosePopUp()
    {
        currentMenu.SetActive(false);
    }
}
