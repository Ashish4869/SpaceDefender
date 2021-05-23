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
        Advertisement.Initialize(GooglePlayID, GameMode);
    }


    public void ShowAds()
    {
        Advertisement.Show();
        
    }


}
