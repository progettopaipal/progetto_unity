using UnityEngine;
using UnityEngine.Advertisements;

public class GestoreAds : MonoBehaviour, IUnityAdsListener   {

    string gameId = "3558873";
    string myPlacementId = "rewardedVideo";
    bool testMode = true;

    //float timeLeft = 5f;
   
    // Initialize the Ads listener and service:
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);

    }
    public void mostra()

    {
        
        Advertisement.Show();
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)

        {
            Advertisement.RemoveListener(this);
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            Advertisement.RemoveListener(this);
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)

        {
            if (Advertisement.IsReady() == true)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
           
        }
    }
   
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            Advertisement.Show();
        }
    }
    
    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
