using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayerSaveProfile : MonoBehaviour
{
    [SerializeField] public SaveData saveData = new SaveData();
    [SerializeField] int tutorialIndex = 1;
    string path;

    public static PlayerSaveProfile Instance { get; private set; }

    void Awake()
    {
        path = Application.persistentDataPath + "/SaveData.json";

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadGame();
    }

    private void Start()
    {
        if (!saveData.notNew)
        {
            SaveGame();
            AsyncLoad.Instance.LoadLevel(tutorialIndex);
        }
    }

#if (UNITY_EDITOR)
    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveGame();
                MainMenuUIWatcher mmui = FindObjectOfType<MainMenuUIWatcher>();
                if (mmui != null)
                {
                    mmui.UpdateUI();
                }
            }
        }
    }
#endif

    public void ResetSave()
    {
        saveData.gems = 0;
        saveData.stars = 0;
        saveData.stageResults = saveData.noResults;
        saveData.highScores = saveData.zeroScore;
        saveData.unlockedSkins = saveData.lockSkins;
        SaveGame();
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);

        Debug.Log(json);
    }

    public void LoadGame()
    {
        if (!File.Exists(path))
        {
            string save = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, save);
        }

        string json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, saveData);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
