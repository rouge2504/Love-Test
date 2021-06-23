using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertisingManager : MonoBehaviour
{
    public static AdvertisingManager instance;

    public void Start()
    {
        instance = this;
    }
    public void ShowAdvertising()
    {
        float rnd = Random.Range(0, 100);
        if (rnd < Constants.porcentToShowAdvertising)
        {
            ShowInterstitialAd();
        }
    }
    public void ShowInterstitialAd()
    {
        YodoManager.instance.ShowInterstitialAd();
    }

    public void ShowRewardVideo()
    {
        YodoManager.instance.ShowRewardVideo();
    }
}
