using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GiftAd : MonoBehaviour
{
    private string RewardedUnitId = "ca-app-pub-6526435970617936/2337563112";
    private RewardedAd rewardedAd;
    public bool adClosed = false;
    public bool adOpen = false;
    private void OnEnable()
    {
        rewardedAd = new RewardedAd(RewardedUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
        rewardedAd.OnUserEarnedReward += HendleUserEarnedReward;
        rewardedAd.OnAdOpening += (sender, args) => { adOpen = true; };
    }
    private void HendleUserEarnedReward(object sender, Reward e)
    {
        adOpen = false;
        adClosed = true;
    }
    public void ShowAd()
    {
        adClosed = false;
        if (rewardedAd.IsLoaded())
            rewardedAd.Show();
    }
}
