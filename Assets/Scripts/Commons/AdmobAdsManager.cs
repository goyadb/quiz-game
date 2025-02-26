using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class AdmobAdsManager : Singleton<AdmobAdsManager>
{

#if UNITY_ANDROID
    private string _bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
    private string _bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
#endif

    private BannerView _bannerView;

    private void Start()
    {
        MobileAds.Initialize(initStatus => 
        {
            // ¹è³Ê±¤°í Ç¥½Ã
            LoadBannerAd();
        });
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    #region Banner Ads

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        if (_bannerView != null)
        {
            // TODO: ¹è³Ê ºä ¼Ò¸ê
            _bannerView.Destroy();
            _bannerView = null;
        }

        _bannerView = new BannerView(_bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    public void LoadBannerAd()
    {
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }

    private void RegisterBannerAdsEventHandler()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    #endregion
}
