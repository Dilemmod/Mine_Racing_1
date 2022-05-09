using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GiftAd : MonoBehaviour
{
    private string RewardedUnitId = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd rewardedAd;
    public bool adClousing =false;
    private void OnEnable()
    {
        rewardedAd = new RewardedAd(RewardedUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
        rewardedAd.OnUserEarnedReward += HendleUserEarnedReward;
    }
    private void HendleUserEarnedReward(object sender, Reward e)
    {
        adClousing = true;
        PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("PlayerCoins")+400);
    }
    public void ShowAd()
    {
        if (rewardedAd.IsLoaded())
            rewardedAd.Show();
    }
}
