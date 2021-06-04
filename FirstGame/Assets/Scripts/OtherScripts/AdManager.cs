using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    string GooglePlayID = "4139333";
    bool GameMode = true;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(GooglePlayID, false);
    }


    public void ShowInterstitialAds()
    {
        if (Advertisement.IsReady() == true)
        {
            Advertisement.Show("Interstitial_Android");
        }
        else return;
        
        
    }


}
