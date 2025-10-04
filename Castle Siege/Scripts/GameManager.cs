using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    bool tutorial, endedGame;
    [SerializeField]
    int stageIndex, stageGemsReward, stageThreeStarsBonus, scoreForOneStar, scoreForTwoStars, scoreForThreeStars, totalPoints;
    [SerializeField] AudioClip winAudio, loseAudio;

    private AudioSource _au;
    int _currentPoints, _finalScore;
    [SerializeField]
    List<GameObject> destructibles, movingObjects, enemies;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        endedGame = false;
        EventManager.Instance.Subscribe("points", GetPoints);
        if (!tutorial) EventManager.Instance.Subscribe("end game", EndGame);
        UIGameWatcher.Instance.endScreen.SetActive(false);
        UIGameWatcher.Instance.finalScore.text = 0.ToString();
        UIGameWatcher.Instance.currentPoints.text = 0.ToString();
        _au = GetComponent<AudioSource>();
        PlayerSaveProfile.Instance.saveData.lastReward = 0;
    }

#if (UNITY_EDITOR)
    private void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Space))
            CalculatePointsNeeded();
    }

    public void ReceiveScore(int score)
    {
        totalPoints += score;
    }

    void CalculatePointsNeeded()
    {
        scoreForOneStar = totalPoints / 3;
        scoreForTwoStars = (totalPoints * 2) / 3;
        scoreForThreeStars = totalPoints - totalPoints / 10;
    }
#endif

    void RemoveEnemyList(GameObject item)
    {
        Enemy e = item.GetComponentInChildren<Enemy>();
        if (e)
        {
            enemies.Remove(item);
        }
    }

    public void AddDesctructibleToList(GameObject item)
    {
        destructibles.Add(item);
        Enemy e = item.GetComponentInChildren<Enemy>();
        if (e)
            enemies.Add(item);

    }

    public void RemoveDestructibleFromList(GameObject item)
    {
        destructibles.Remove(item);
        RemoveEnemyList(item);
        RemoveMovingItem(item);
    }

    public void RemoveMovingItem(GameObject item)
    {
        movingObjects.Remove(item);

        if (movingObjects.Count == 0)
        {
            _currentPoints = 0;

            UIGameWatcher.Instance.currentPoints.text = _currentPoints.ToString();
            if (destructibles.Count == 0 && !endedGame || enemies.Count == 0 && !endedGame)
            {
                EventManager.Instance.Trigger("end game");
                endedGame = true;
            }
        }
    }

    public void AddMovingItem(GameObject item)
    {
        movingObjects.Add(item);
    }

    void GetPoints(params object[] parameters)
    {
        _finalScore += (int)parameters[0];
        _currentPoints += (int)parameters[0];
        UIGameWatcher.Instance.finalScore.text = _finalScore.ToString();
        UIGameWatcher.Instance.currentPoints.text = _currentPoints.ToString();
    }

    void EndGame(params object[] parameters)
    {
        if (!tutorial)
        {
            SaveData psd = PlayerSaveProfile.Instance.saveData;
            UIGameWatcher ui = UIGameWatcher.Instance;

            if (psd.highScores[stageIndex] < _finalScore)
                psd.highScores[stageIndex] = _finalScore;

            if (enemies.Count > 0)
            {
                ui.loseText.gameObject.SetActive(true);
                EventManager.Instance.Trigger("lose");
                _au.PlayOneShot(loseAudio);

            }
            if (enemies.Count == 0)
            {
                if (_finalScore >= scoreForThreeStars)
                {
                    if (psd.stageResults[stageIndex] != SaveData.StageResult.three)
                    {
                        if (psd.stageResults[stageIndex] == SaveData.StageResult.zero)
                        {
                            psd.stars += 3;
                        }
                        if (psd.stageResults[stageIndex] == SaveData.StageResult.one)
                        {
                            psd.stars += 2;
                        }
                        if (psd.stageResults[stageIndex] == SaveData.StageResult.two)
                        {
                            psd.stars++;
                        }

                        psd.stageResults[stageIndex] = SaveData.StageResult.three;

                        GiveBonus();
                    }

                    GiveRewards();
                    for (int i = 0; i < 3; i++)
                    {
                        ui.starsImages[i].gameObject.SetActive(true);
                    }
                }

                if (_finalScore < scoreForThreeStars && _finalScore >= scoreForTwoStars)
                {
                    if (psd.stageResults[stageIndex] != SaveData.StageResult.three || psd.stageResults[stageIndex] != SaveData.StageResult.two)
                    {
                        if (psd.stageResults[stageIndex] == SaveData.StageResult.zero)
                        {
                            psd.stars += 2;
                        }

                        if (psd.stageResults[stageIndex] == SaveData.StageResult.one)
                        {
                            psd.stars++;
                        }

                        psd.stageResults[stageIndex] = SaveData.StageResult.two;
                    }

                    GiveRewards();
                    for (int i = 0; i < 2; i++)
                    {
                        ui.starsImages[i].gameObject.SetActive(true);
                    }
                }

                if (_finalScore < scoreForTwoStars && _finalScore >= scoreForOneStar)
                {
                    if (psd.stageResults[stageIndex] == SaveData.StageResult.zero)
                    {
                        psd.stageResults[stageIndex] = SaveData.StageResult.one;
                        psd.stars++;
                    }

                    GiveRewards();
                    ui.starsImages[0].gameObject.SetActive(true);
                }
            }
            
            else if (_finalScore < scoreForOneStar)
            {
                ui.loseText.gameObject.SetActive(true);
                EventManager.Instance.Trigger("lose");
                _au.PlayOneShot(loseAudio);
            }

            ui.endScreen.SetActive(true);
            ui.endScreenFinalScore.text = _finalScore.ToString();
            ui.endScreenHighScore.text = psd.highScores[stageIndex].ToString();
            PlayerSaveProfile.Instance.SaveGame();
        }
    }

    void GiveRewards()
    {
        EventManager.Instance.Trigger("win");
        UIGameWatcher.Instance.gemsReward.gameObject.SetActive(true);
        UIGameWatcher.Instance.winText.gameObject.SetActive(true);
        UIGameWatcher.Instance.gemsReward.text = stageGemsReward.ToString();
        PlayerSaveProfile.Instance.saveData.gems += stageGemsReward;
        PlayerSaveProfile.Instance.saveData.lastReward += stageGemsReward;
        PlayerSaveProfile.Instance.saveData.gemPopUp = true;
        FeedbackManager.Instance.PlayWinParticle();
        _au.PlayOneShot(winAudio);
    }

    void GiveBonus()
    {
        UIGameWatcher.Instance.gemsBonus.gameObject.SetActive(true);
        UIGameWatcher.Instance.gemsBonus.text = stageThreeStarsBonus.ToString();
        PlayerSaveProfile.Instance.saveData.gems += stageThreeStarsBonus;
        PlayerSaveProfile.Instance.saveData.lastReward += stageThreeStarsBonus;
    }
}
