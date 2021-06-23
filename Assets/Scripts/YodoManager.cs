using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yodo1.MAS;
public class YodoManager : MonoBehaviour
{
    public static YodoManager instance;

    public enum MODE { INTERSTITIAL, VIDEO}

    public static MODE mode;
    // Start is called before the first frame update
    void Awake()
    {
        
        #region YODO    

        Yodo1U3dMas.InitializeSdk();
        BannerAdsCallBack();
        RewardedVideoCallBack();
        InteristitialCallBack();
        #endregion
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Yodo1U3dMas.SetGDPR(true);

        bool isLoaded = Yodo1U3dMas.IsBannerAdLoaded();
        int align = Yodo1U3dBannerAlign.BannerBottom /*| Yodo1U3dBannerAlign.BannerHorizontalCenter*/;
        Yodo1U3dMas.ShowBannerAd(align);
        //Yodo1U3dMas.ShowBannerAd();

    }

    // Update is called once per frame
    void Update()
    {
        /*if (Yodo1U3dMas.IsBannerAdLoaded())
        {
            int align = Yodo1U3dBannerAlign.BannerBottom | Yodo1U3dBannerAlign.BannerHorizontalCenter;
            Yodo1U3dMas.ShowBannerAd();
        }*/

        //InteristitialCallBack();
    }

    public void ShowInterstitialAd()
    {
        Yodo1U3dMas.ShowInterstitialAd();
    }

    public bool isLoadedInterstitialAd()
    {
        return Yodo1U3dMas.IsInterstitialAdLoaded();
    }

    public void ShowRewardVideo()
    {
        //RewardedVideo();
        Yodo1U3dMas.ShowRewardedAd();
    }

    public bool isLoadedVideoAd()
    {
        return Yodo1U3dMas.IsRewardedAdLoaded();
    }

    public void RewardedVideoCallBack()
    {
        Yodo1U3dMas.SetRewardedAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) => {
            Debug.Log("[Yodo1 Mas] RewardVideoDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Reward video ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Reward video ad has shown successful.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Reward video ad error, " + error);
                    break;
                case Yodo1U3dAdEvent.AdReward:
                    Debug.Log("[Yodo1 Mas] Reward video ad reward, give rewards to the player.");
                    break;
            }

        });
    }

    void BannerAdsCallBack()
    {
        Yodo1U3dMas.SetBannerAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) => {
            Debug.Log("[Yodo1 Mas] BannerdDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Banner ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Banner ad has been shown.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Banner ad error, " + error.ToString());
                    Yodo1U3dMas.ShowBannerAd();
                    break;
            }
        });
    }

    void InteristitialCallBack()
    {
        Yodo1U3dMas.SetInterstitialAdDelegate((Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error) => {
            Debug.Log("[Yodo1 Mas] InterstitialAdDelegate:" + adEvent.ToString() + "\n" + error.ToString());
            switch (adEvent)
            {
                case Yodo1U3dAdEvent.AdClosed:
                    Debug.Log("[Yodo1 Mas] Interstital ad has been closed.");
                    break;
                case Yodo1U3dAdEvent.AdOpened:
                    Debug.Log("[Yodo1 Mas] Interstital ad has been shown.");
                    break;
                case Yodo1U3dAdEvent.AdError:
                    Debug.Log("[Yodo1 Mas] Interstital ad error, " + error.ToString());
                    break;
            }
        });
    }
}
