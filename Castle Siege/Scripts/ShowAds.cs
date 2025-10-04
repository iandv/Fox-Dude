using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShowAds : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] string gameID = "5298343";
    [SerializeField] string adID = "Rewarded_Android";

    // Start is called before the first frame update
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
        if(placementId == adID) Debug.Log("Is ready!");
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
            if(showResult == ShowResult.Finished)
            {
                Debug.Log("Max rewards");
            }
            else if (showResult == ShowResult.Skipped)
            {
                Debug.Log("Half rewards");
            }
            else
            {
                Debug.Log("No rewards");
            }
        }
    }
}
