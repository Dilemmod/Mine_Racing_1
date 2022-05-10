using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterAd : MonoBehaviour
{
    private InterstitialAd interstialAd;
    private string interstialUnitID = "ca-app-pub-6526435970617936/7568626928";
    public bool adClose = false;
    private void Start()
    {
        interstialAd.OnAdClosed += (sender, args) => { adClose = true;}; 
    }
    private void OnEnable()
    {
        interstialAd = new InterstitialAd(interstialUnitID);
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstialAd.LoadAd(adRequest);
    }
    public void ShowAd()
    {
        if (interstialAd.IsLoaded())
            interstialAd.Show();
    }
    
}
