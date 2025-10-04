using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using TMPro;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] int partialReward, fullReward;
    [SerializeField] GameObject rewardPopUp;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] Button _showAdButton;
    [SerializeField] string adID = "Android_Rewarded";

    [SerializeField] string gameID = "3908349";
    string _adUnitId = null; // This will remain null for unsupported platforms

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID);
    }

    public void ShowAd()
    {
        if (!Advertisement.IsReady()) return;

        Advertisement.Show(adID);
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == adID) Debug.Log("Is ready!");
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == adID)
        {
            if (showResult == ShowResult.Finished)
            {
                StartCoroutine(WaitFrame(fullReward));
                Debug.Log("Unity Ads Rewarded Ad Completed");
                Debug.Log("Max rewards");
            }
            else if (showResult == ShowResult.Skipped)
            {
                StartCoroutine(WaitFrame(partialReward));
                Debug.Log("Unity Ads Rewarded Ad Skipped");
                Debug.Log("Partial rewards");
            }
            else
            {
                Debug.Log("No rewards");
            }
        }
    }

    IEnumerator WaitFrame(int reward)
    {
        yield return 0;
        PlayerSaveProfile.Instance.saveData.gems += reward;
        PlayerSaveProfile.Instance.SaveGame();
        MainMenuUIWatcher.Instance.UpdateUI();
        rewardPopUp.SetActive(true);
        rewardText.text = "+" + reward.ToString();
    }
}