using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PromptScript : MonoBehaviour
{
    public int sceneIndex;
    public void HideObject()
    {
        gameObject.SetActive(false);
    }

    public void LoadNewSceneAsync()
    {
        PlayerSaveProfile.Instance.saveData.notNew = true;
        PlayerSaveProfile.Instance.SaveGame();
        AsyncLoad.Instance.LoadLevel(0);
    }
}
